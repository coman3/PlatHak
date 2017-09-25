using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlatHak.Common.Network.Sockets.Extensions;
using Sockets.Plugin.Abstractions;
using Splat;

namespace PlatHak.Common.Network.Sockets.Messaging
{
    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged
    {
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            var th = this;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new [] { item }));
        }

        public new T Dequeue()
        {
            var item = base.Dequeue();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { item }));
            return item;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    public class JsonProtocolTaskMessenger<TMessage> : IEnableLogger where TMessage : class
    {
        readonly ITcpSocketClient _client;
        private Task _receiveTask;
        public event EventHandler<MessengerDisconnectedEventArgs> Disconnected;
        public event EventHandler<MessengerMessageEventArgs<TMessage>> MessageReceived;

        private readonly ObservableQueue<JsonProtocolQueueItem<TMessage>> _sendQueue;
        private readonly ObservableQueue<TMessage> _receiveQueue;
        public List<TMessage> Messages => _receiveQueue.ToList();

        private CancellationTokenSource _executeCancellationSource;
        public List<Assembly> AdditionalTypeResolutionAssemblies { get; set; } = new List<Assembly>();

        public JsonProtocolTaskMessenger(ITcpSocketClient client)
        {
            _client = client;
            _receiveQueue = new ObservableQueue<TMessage>();
            _sendQueue = new ObservableQueue<JsonProtocolQueueItem<TMessage>>();
            _sendQueue.CollectionChanged += SendQueueOnCollectionChanged;
        }

        private async void SendQueueOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if(notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
                await SendWorker(_sendQueue.Dequeue());
        }

        public void Send(TMessage message)
        {
            var wrapper = new JsonProtocolQueueItem<TMessage>
            {
                MessageType = JsonProtocolMessengerMessageType.StandardMessage,
                Payload = message
            };

            _sendQueue.Enqueue(wrapper);
        }

        public async Task Disconnect(DisconnectionType disconnectionType)
        {
            var wrapper = new JsonProtocolDisconnectionQueueItem<TMessage>
            {
                MessageType = JsonProtocolMessengerMessageType.DisconnectMessage,
                DisconnectionType = disconnectionType
            };

            _sendQueue.Enqueue(wrapper);
            // this lets you await the *sending* of the disconnection
            // (i.e. not just the queuing of it)
            // this way we dont' actually disconnect until after we have told people
            // TODO: in case it somehow doesn't happen, timeout after a bit
            await wrapper.Delivered;
            StopExecuting();
        }

        public void StartExecuting()
        {
            if (_executeCancellationSource != null && !_executeCancellationSource.IsCancellationRequested)
                _executeCancellationSource.Cancel();

            _executeCancellationSource = new CancellationTokenSource();

            // json object protocol
            // first byte = messageType { std, disconnect, ... }
            // next 4 typeNameLength - n
            // next 4 = messageLength - m
            // next n+m = type+message
            //await Worker();
            _receiveTask =  Task.Run(Worker, _executeCancellationSource.Token);
        }

        private async Task SendWorker(JsonProtocolQueueItem<TMessage> queueItem)
        {
            var canceller = _executeCancellationSource.Token;
            if (queueItem.MessageType != JsonProtocolMessengerMessageType.StandardMessage &&
                queueItem.MessageType != JsonProtocolMessengerMessageType.DisconnectMessage)
                throw new InvalidOperationException(
                    "There's no code for sending other message types (please feel free to add some)");

            switch (queueItem.MessageType)
            {
                case JsonProtocolMessengerMessageType.StandardMessage:
                {
                    this.Log().Debug($"SEND: {queueItem.Payload.AsJson()}");

                    var payload = queueItem.Payload;

                    var typeNameBytes = payload.GetType().FullName.AsUTF8ByteArray();
                    var messageBytes = payload.AsJson().AsUTF8ByteArray();

                    var typeNameSize = typeNameBytes.Length.AsByteArray();
                    var messageSize = messageBytes.Length.AsByteArray();

                    var allBytes = new[]
                        {
                            new[] {(byte) JsonProtocolMessengerMessageType.StandardMessage},
                            typeNameSize,
                            messageSize,
                            typeNameBytes,
                            messageBytes
                        }
                        .SelectMany(b => b)
                        .ToArray();
                    Debug.WriteLine("Sending Packet....");
                    await _client.WriteStream.WriteAsync(allBytes, 0, allBytes.Length, canceller);
                    await _client.WriteStream.FlushAsync(canceller);

                    break;
                }

                case JsonProtocolMessengerMessageType.DisconnectMessage:
                {
                    var dcItem = queueItem as JsonProtocolDisconnectionQueueItem<TMessage>;
                    if (dcItem == null) break;
                    this.Log().Debug($"SEND DISC: {dcItem?.DisconnectionType}");

                    var allBytes = new[]
                    {
                        (byte) JsonProtocolMessengerMessageType.DisconnectMessage,
                        (byte) dcItem.DisconnectionType
                    };

                    await _client.WriteStream.WriteAsync(allBytes, 0, allBytes.Length, canceller);
                    await _client.WriteStream.FlushAsync(canceller);

                    dcItem.DidSend();

                    break;
                }
            }
        }

        private async Task Worker()
        {
            var canceller = _executeCancellationSource.Token;

            while (!canceller.IsCancellationRequested)
            {
                byte[] messageTypeBuf = new byte[1];
                var count = await _client.ReadStream.ReadAsync(messageTypeBuf, 0, 1, canceller);

                if (count == 0)
                {
                    _executeCancellationSource.Cancel();

                    this.Log().Error("Unexpected disconnection");

                    Disconnected?.Invoke(this, new MessengerDisconnectedEventArgs(DisconnectionType.Unexpected));

                    return;
                }

                var messageType = (JsonProtocolMessengerMessageType)messageTypeBuf[0];

                switch (messageType)
                {
                    case JsonProtocolMessengerMessageType.StandardMessage:

                        var typeNameLength = (await _client.ReadStream.ReadBytesAsync(sizeof(int), canceller)).AsInt32();
                        var messageLength = (await _client.ReadStream.ReadBytesAsync(sizeof(int), canceller)).AsInt32();

                        var typeNameBytes = await _client.ReadStream.ReadBytesAsync(typeNameLength, canceller);
                        var messageBytes = await _client.ReadStream.ReadBytesAsync(messageLength, canceller);

                        var typeName = typeNameBytes.AsUTF8String();
                        var messageJson = messageBytes.AsUTF8String();

                        var type = Type.GetType(typeName) ??
                                   AdditionalTypeResolutionAssemblies
                                       .Select(a => Type.GetType($"{typeName}, {a.FullName}"))
                                       .FirstOrDefault(t => t != null); ;

                        if (type == null)
                        {
                            this.Log().Warn($"Received a message of type '{typeName}' but couldn't resolve it using GetType().");
                            continue;
                        }

                        var msg = JsonConvert.DeserializeObject(messageJson, type) as TMessage;

                        this.Log().Debug($"RECV: {msg.AsJson()}");

                        _receiveQueue.Enqueue(msg);
                        MessageReceived?.Invoke(this, new MessengerMessageEventArgs<TMessage>(msg));

                        break;

                    case JsonProtocolMessengerMessageType.DisconnectMessage:
                        var disconnectionType = (DisconnectionType)(await _client.ReadStream.ReadByteAsync(canceller));
                        this.Log().Debug(String.Format("RECV DISC: {0}", disconnectionType));

                        Disconnected?.Invoke(this, new MessengerDisconnectedEventArgs(disconnectionType));

                        StopExecuting();

                        return;
                }

            }
        }

        public void StopExecuting()
        {
            _receiveQueue.Clear();
            _sendQueue.Clear();
            _executeCancellationSource.Cancel();
            _executeCancellationSource = null;
            _receiveTask.Wait();
        }
    }

    public class MessengerMessageEventArgs<TMessage> : EventArgs
        where TMessage : class
    {
        public TMessage Message { get; set; }

        public MessengerMessageEventArgs(TMessage message)
        {
            Message = message;
        }
    }
}