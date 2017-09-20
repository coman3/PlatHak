//#define MultiThread

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO.Ports;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using PlatHak.Common.Maths;
//using PlatHak.Common.Network;
//using PlatHak.Common.Objects;
//using PlatHak.Common.World;
//using PlatHak.Server.Common.Extentions;
//using PlatHak.Server.Network;
//using Color = PlatHak.Common.Drawing.Color;
//using Size = PlatHak.Common.Maths.Size;

//namespace PlatHak.Server.Common
//{
//    public class ServerWorldManager
//    {
//        public World World { get; set; }
//        public WorldSaver WorldSaver { get; set; }
//        public WebSocketServer Server { get; set; }
//        public int PreloadClusterWidth { get; set; }
//        public int SpawnChunkInformRadius { get; set; }
//        public Dictionary<UserClient, List<VectorLong2>> RequestedChunks { get; set; }
//        public Queue<VectorLong2> LoadingClusters { get; set; }
//        private readonly CancellationToken _chunkManagerCancellationToken;
//        private Task _chunkManagerTask;

//        public ServerWorldManager(WebSocketServer socketServer, World world)
//        {
//            World = world;
//            Server = socketServer;

//            Server.OnPacketReceived += Server_OnPacketReceived;
//            Server.OnClientLoginHandshakeSuccess += Server_OnClientLoginHandshakeSuccess;
//            Server.OnClientLoaded += Server_OnClientLoaded;
//            Server.OnUpdate += Server_OnUpdate;

//            SpawnChunkInformRadius = 4;
//            RequestedChunks = new Dictionary<UserClient, List<VectorLong2>>();
//            LoadingClusters = new Queue<VectorLong2>();
//            _chunkManagerCancellationToken = new CancellationToken();

//            StartDynamicLoader();
//        }

//        private void Server_OnUpdate()
//        {
//            //foreach (var player in World.Players)
//            //{
//            //    //UpdatePlayerPosistion(player, player.UpdatePosistion());
//            //}
//        }

//        private void Server_OnClientLoaded(UserClient client)
//        {
//            Console.WriteLine($"{client.SessionId} ({client.Username}) game has loaded. Sending world...");
//            client.Send(new WorldConfigPacket(World.WorldConfig));
//            client.Send(new CreateEntityPacket(client.Player));
//            var chunkIn = World.GetChunkCordsFromPosition(client.Player.Position);
//            lock (RequestedChunks)
//            {

//                RequestedChunks[client] = new List<VectorLong2>();
//                RadialScan.AllPoints(chunkIn, SpawnChunkInformRadius,
//                    (x, y) =>
//                    {
//                        RequestedChunks[client].Add(new VectorLong2(x, y));
//                    });
//            }
//        }

//        private void Server_OnClientLoginHandshakeSuccess(UserClient session)
//        {
//            if (session.Player == null)
//            {
//                var random = new Random();
//                session.Player = new Player
//                {
//                    Position = new VectorLong2(1044, 1500),
//                    Username =  session.Username
//                };
//                World.Entities.Add(session.Player);
//            }
//        }

//        private void StartDynamicLoader()
//        {
//            _chunkManagerTask = Repeat.Interval(TimeSpan.FromMilliseconds(250), ChunkManagerTaskAction,
//                _chunkManagerCancellationToken);
//        }

//        private void ChunkManagerTaskAction()
//        {
//            lock (RequestedChunks)
//                lock (LoadingClusters)
//            {
//                foreach (var pair in RequestedChunks)
//                {
//                    foreach (var chunkPos in pair.Value)
//                    {
//                        if (!pair.Key.SentChunks.Any(c => c.Equals(chunkPos)))
//                            //have not sent the chunk already
//                        {
//                            if (World.Chunks[chunkPos.X, chunkPos.Y] == null)
//                            {
//                                var clusterPos = chunkPos / WorldSaver.ChunksPerFile;
//                                if (!LoadingClusters.Any(c => c.Equals(clusterPos)))
//                                    LoadingClusters.Enqueue(clusterPos);
//                                continue;
//                            }
//                            pair.Key.SentChunks.Add(chunkPos);
//                            Console.WriteLine("Send Chunk: " + chunkPos);
//                            pair.Key.Send(new ChunkPacket(World.Chunks[chunkPos.X, chunkPos.Y]));
//                        }
//                    }
//                }
//            }
//            lock (LoadingClusters)
//            {
//                while (LoadingClusters.Count > 0)
//                {
//                    var item = LoadingClusters.Dequeue();
//                    WorldSaver.LoadChunkCluster(item);
//                }
//            }
//        }

//        private void Server_OnPacketReceived(UserClient client, Packet packet)
//        {
//            packet.DoIfIsType<ChunkRequestPacket>(requestPacket =>
//            {
//                lock (RequestedChunks)
//                {
//                    RequestedChunks[client].Add(requestPacket.ChunkPosistion);
//                }
//            });
//        }

//        public Chunk GetChunk(VectorLong2 pos)
//        {
//            if (World.Chunks[pos.X, pos.Y] == null)
//            {
//                WorldSaver.LoadChunkCluster(pos);
//            }
//            return World.Chunks[pos.X, pos.Y];
//        }

//        #region World Loading

//        public void Load(Bitmap blockBitmap)
//        {
//            WorldSaver = new WorldSaver(World, new Size(24, 24));

//            var count = 0;
//            var total = WorldSaver.Size.Width * WorldSaver.Size.Height;
//            double totalTime = 0;

//            #region LoopInit

//#if MultiThread
//            Parallel.For(0, WorldSaver.Size.Width, sx =>
//            {
//                Parallel.For(0, WorldSaver.Size.Height, sy =>
//                {
//#else
//            for (int sx = 0; sx < WorldSaver.Size.Width; sx++)
//            {
//                for (int sy = 0; sy < WorldSaver.Size.Height; sy++)
//                {
//#endif

//                    #endregion
//                    count++;
//                    var timeTaken = LoopMethod(blockBitmap, sx, sy);

//                    totalTime += timeTaken;
//                    Console.Clear();
//                    Console.SetCursorPosition(0, 49);
//                    Console.Write($"Cluster: {count} / {total} ({count / (float) total * 100} %)    ");
//                    Console.SetCursorPosition(0, 50);
//                    var timespan = TimeSpan.FromSeconds(totalTime / count);
//                    Console.Write($"Average Cluster Generation Time: {timespan.ToString(@"ss\:fffffff")}    ");
//                    Console.SetCursorPosition(0, 51);
//                    var timeLeft = TimeSpan.FromSeconds((total - count) * timespan.TotalSeconds);
//                    Console.Write($"Estimated Time Left: {timeLeft.ToString(@"hh\:mm\:ss\:fffffff")}    ");
//                    Console.SetCursorPosition(0, 0);
//            #region LoopEnd

//#if MultiThread
//                });
//            });
//#else
//                }
//            }
//#endif

//            #endregion

//            Console.Clear();
//        }

//        private double LoopMethod(Bitmap blockBitmap, int sx, int sy)
//        {
//            var stopwatch = Stopwatch.StartNew();
//            if (!WorldSaver.Exists(sx, sy))
//            {
//                var blockData = new bool[WorldSaver.ChunksPerFile.Width, WorldSaver.ChunksPerFile.Height];
//                lock (blockBitmap)
//                {
//                    for (int x = 0; x < WorldSaver.ChunksPerFile.Width; x++)
//                    {
//                        for (int y = 0; y < WorldSaver.ChunksPerFile.Height; y++)
//                        {
//                            var ax = sx * WorldSaver.ChunksPerFile.Width + x;
//                            var ay = sy * WorldSaver.ChunksPerFile.Height + y;

//                            var color = blockBitmap.GetPixel(ax, ay);
//                            blockData[x, y] = color.A > 50;
//                            //If pixel is greater than 50 alpha, it is a full chunk
//                        }
//                    }
//                }
//                GenerateWorldChunk(sx, sy, WorldSaver, blockData);
//            }
//            stopwatch.Stop();
//            return stopwatch.Elapsed.TotalSeconds;
//        }

//        private void GenerateWorldChunk(int sx, int sy, WorldSaver worldSaver, bool[,] blockdata)
//        {
//            var grids = new ChunkCluster(worldSaver.ChunksPerFile);
//            for (int x = 0; x < worldSaver.ChunksPerFile.Width; x++)
//            {
//                for (int y = 0; y < worldSaver.ChunksPerFile.Height; y++)
//                {
//                    var ax = sx * worldSaver.ChunksPerFile.Width + x;
//                    var ay = sy * worldSaver.ChunksPerFile.Height + y;
//                    grids[x, y] = new Chunk(new VectorLong2(ax, ay), World.WorldConfig);
//                    for (int cx = 0; cx < World.WorldConfig.ChunkSize.Width; cx++)
//                    {
//                        for (int cy = 0; cy < World.WorldConfig.ChunkSize.Height; cy++)
//                        {
//                            if (blockdata[x, y])
//                                grids[x, y].AddGridItem(cx, cy, new Tile());
//                        }
//                    }
//                }
//            }
//            worldSaver.Save(sx, sy, grids);
//            grids.Dispose();
//        }

//        #endregion
//    }
//}