﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PlatHak.Common.Network.Sockets.Discovery.Base;
using PlatHak.Common.Network.Sockets.Discovery.ServiceDefinition;
using Sockets.Plugin.Abstractions;

namespace PlatHak.Common.Network.Sockets.Discovery
{
    /// <summary>
    ///     Acts as the service discoverer and sends discovery requests according to the protocol defined by
    ///     `TServiceDefinition`.
    /// </summary>
    /// <typeparam name="TServiceDefinition"></typeparam>
    /// <typeparam name="TRequestFormat"></typeparam>
    /// <typeparam name="TPayloadFormat"></typeparam>
    public class ServiceDiscoverer<TServiceDefinition, TRequestFormat, TPayloadFormat> :
        ServiceDiscovererBase<TServiceDefinition>
        where TServiceDefinition : TypedServiceDefinition<TRequestFormat, TPayloadFormat>
        where TPayloadFormat : IDiscoveryPayload
    {
        public ServiceDiscoverer(TServiceDefinition definition) : base(definition)
        {
        }

        protected override void OnMessageReceived(object sender, UdpSocketMessageReceivedEventArgs e)
        {
            var payload = _serviceDefinition.BytesToPayload(e.ByteData);

            // add the remote host data
            var port = -1;
            Int32.TryParse(e.RemotePort, out port);

            payload.RemoteAddress = e.RemoteAddress;
            payload.RemotePort = port;

            // pump the discovery response
            _discoveredServices.OnNext(payload);
        }

        private readonly Subject<TPayloadFormat> _discoveredServices = new Subject<TPayloadFormat>();

        public IObservable<TPayloadFormat> DiscoveredServices
        {
            get { return _discoveredServices.AsObservable(); }
        }
    }
}
