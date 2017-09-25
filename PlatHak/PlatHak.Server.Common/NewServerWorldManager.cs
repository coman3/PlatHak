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

            Console.WriteLine("Loading World...");
            WorldSaver.LoadWorld(ManagerConfig.WorldSavePath);
            Console.WriteLine("World Loaded. (Total Blocks: {0})", World.Blocks.Count);

            Console.WriteLine("Seeing what world tiles we need...");
            var requiredWorldDataTles =
                WorldDataManager.GetRequiredDataTiles(WorldDataManager.Citys.MelbourneAustralia);
            var unloadedWorldDataTiles = new List<WorldDataTile>();
            foreach (var tile in requiredWorldDataTles)
            {
                var globalPos =
                    World.WorldConfig.GetGlobalPosistionFromLatLon(new Vector2((float) tile.CenterLatitudedec,
                        (float) tile.CenterLongitudedec));
                var blockPos = World.WorldConfig.GetBlockLocalPosistion(globalPos);
                if (!World.Blocks.ContainsKey(blockPos))
                {
                    unloadedWorldDataTiles.Add(tile);
                }
            }
            Console.WriteLine("Need a total of {0} world tiles.", unloadedWorldDataTiles.Count);

            if (unloadedWorldDataTiles.Count > 0)
            {
                Console.WriteLine("Downloading World Data...");
                await WorldDataManager.DownloadWorldData(unloadedWorldDataTiles.ToArray());
                Console.WriteLine("Downloaded files needed.");

                Console.WriteLine("Loading Terrain...");
                var images = await WorldDataManager.Generate(unloadedWorldDataTiles.ToArray());
                Console.WriteLine("Genrating Terrain...");
                var cluster = await WorldSaver.LoadCluster(images);
                Console.WriteLine("Terrain Generated.");
                foreach (var clust in cluster)
                {
                    World.SetCluster(clust);
                }

                Console.WriteLine("Saving World...");
                //TODO: Save only changed...?
                WorldSaver.SaveWorld(ManagerConfig.WorldSavePath);
                Console.WriteLine("World Saved..");
            }
            else
            {
                Console.WriteLine("Terrain Already Generated. Skiping..");
            }


            Console.WriteLine("Total Blocks Loaded: {0}", World.Blocks?.Count);
            stopWatch.Stop();
            Console.WriteLine($"Finished Initializing World. Took: {stopWatch.Elapsed}s");
        }
    }

    public class WorldManagerConfig
    {
        public int SpawnInformChunkRaduis { get; set; }
        public string WorldDataPath { get; set; }
        public string WorldSavePath { get; set; }

        public WorldManagerConfig(int spawnInformChunkRaduis, string worldDataPath, string worldSavePath)
        {
            SpawnInformChunkRaduis = spawnInformChunkRaduis;
            WorldDataPath = worldDataPath;
            WorldSavePath = worldSavePath;
        }
    }
}