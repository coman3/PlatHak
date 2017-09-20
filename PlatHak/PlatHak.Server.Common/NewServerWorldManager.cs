using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlatHak.Common.Maths;
using PlatHak.Common.Network;
using PlatHak.Common.Objects;
using PlatHak.Common.World;
using PlatHak.Server.Common.Extentions;
using PlatHak.Server.Common.Models;
using PlatHak.Server.Network;
using PlatHak.Server.WorldData;

namespace PlatHak.Server.Common
{
    public class NewServerWorldManager
    {
        public World World { get; set; }
        public WorldSaver WorldSaver { get; set; }

        public WorldManagerConfig ManagerConfig { get; set; }
        public WorldDataManager WorldDataManager { get; set; }

        public WebSocketServer Server { get; set; }
        

        /// <summary>
        /// Collection of Request Chunks for each client. In Global 2D Posistion
        /// </summary>
        public ClientDictionary<VectorLong2> RequestedChunks { get; set; }

        public NewServerWorldManager(WebSocketServer socketServer, World world, WorldManagerConfig managerConfig)
        {
            World = world;
            Server = socketServer;
            WorldSaver = new WorldSaver(World);
            ManagerConfig = managerConfig;

            WorldDataManager = new WorldDataManager(ManagerConfig.WorldDataPath);
            
            Server.OnPacketReceived += Server_OnPacketReceived;
            Server.OnClientLoginHandshakeSuccess += Server_OnClientLoginHandshakeSuccess;
            Server.OnClientLoaded += Server_OnClientLoaded;
            Server.OnUpdate += Server_OnUpdate;
            
            RequestedChunks = new ClientDictionary<VectorLong2>();
            RequestedChunks.CollectionChanged += RequestedChunks_CollectionChanged;

        }

        private void RequestedChunks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var pair in RequestedChunks)
                //foreach user client
            {

                foreach (var chunkPos in pair.Value)
                    //foreach requested chunk pos
                {

                   
                }
            }

        }

        private void Server_OnUpdate()
        {

        }

        private void Server_OnClientLoaded(UserClient client)
        {
            Console.WriteLine($"{client.SessionId} ({client.Username}) game has loaded. Sending world...");
            client.Send(new WorldConfigPacket(World.WorldConfig));
            
        }

        private void Server_OnClientLoginHandshakeSuccess(UserClient session)
        {
            if (session.Player == null)
            {
                var random = new Random();
                session.Player = new Player
                {
                    Position = new VectorLong2(1044, 1500),
                    Username = session.Username
                };
                World.Entities.Add(session.Player);
            }
        }

        private void Server_OnPacketReceived(UserClient client, Packet packet)
        {
            packet.DoIfIsType<ChunkRequestPacket>(requestPacket =>
            {
                lock (RequestedChunks)
                {
                    RequestedChunks[client].Add(requestPacket.ChunkPosistion);
                }
            });
        }

        public async Task Initialize()
        {
            var stopWatch = Stopwatch.StartNew();

            Console.WriteLine("Loading World Data...");
            await WorldDataManager.LoadFile();

            Console.WriteLine("Preloading Melbourne VIC, Australia");
            //await WorldDataManager.Preload(WorldDataManager.Citys.MelbourneAustralia);

            //var image = await WorldDataManager.Load(WorldDataManager.Citys.MelbourneAustralia);

            //var cluster = await WorldSaver.LoadCluster(image);

            //foreach (var clust in cluster)
            //{
            //    World.SetCluster(clust);
            //}

            WorldSaver.SaveWorld(Path.Combine(Environment.CurrentDirectory, "Chunks"));
            WorldSaver.LoadWorld(Path.Combine(Environment.CurrentDirectory, "Chunks"));
            Console.WriteLine(World.Blocks?.Count);
            stopWatch.Stop();
            Console.WriteLine($"Finished Initializing. (Took: {stopWatch.Elapsed})");
        }
    }

    public class WorldManagerConfig
    {
        public int SpawnInformChunkRaduis { get; set; }
        public string WorldDataPath { get; set; }

        public WorldManagerConfig(int spawnInformChunkRaduis, string worldDataPath)
        {
            SpawnInformChunkRaduis = spawnInformChunkRaduis;
            WorldDataPath = worldDataPath;
        }
    }
}