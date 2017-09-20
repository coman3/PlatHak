using System;
using System.Collections.Generic;
using System.IO;
using PlatHak.Common.Interfaces;
using PlatHak.Common.Maths;

namespace PlatHak.Common.World
{
    public class Block : IDisposable, ISerialize
    {
        public Rectangle Bounds { get; set; }
        public Dictionary<VectorLong2, Cluster> Clusters { get; set; }
        public Size Size => Bounds.Size;
        private WorldConfig WorldConfig { get; }

        public Block(VectorLong2 globalPosistion, WorldConfig config)
        {
            WorldConfig = config;

            Bounds = new Rectangle(globalPosistion, config.BlockSize * config.ClusterSize * config.ChunkSize);
            Clusters = new Dictionary<VectorLong2, Cluster>(Convert.ToInt32(config.BlockSize.Width * config.BlockSize.Height));
        }

        public Block(WorldConfig config, Stream stream)
        {
            WorldConfig = config;
            FromStream(stream);
        }

        public void Dispose()
        {
            if (Clusters == null) return;
            foreach (var gridItem in Clusters)
            {
                gridItem.Value?.Dispose();
            }
            Clusters = null;
        }

        public void ToStream(Stream stream)
        {
            Bounds.ToStream(stream);
            stream.Write(BitConverter.GetBytes(Clusters.Count), 0, sizeof(int));
            foreach (var cluster in Clusters)
            {
                cluster.Value.ToStream(stream);
            }
        }

        public void FromStream(Stream stream)
        {
            Bounds = new Rectangle(stream);
            byte[] bytes = new byte[sizeof(long)];
            stream.Read(bytes, 0, sizeof(int));
            var clusterCount = BitConverter.ToInt32(bytes, 0);
            Clusters = new Dictionary<VectorLong2, Cluster>();
            for (int i = 0; i < clusterCount; i++)
            {
                var cluster = new Cluster(WorldConfig, stream);
                Clusters.Add(WorldConfig.GetClusterLocalPosistion(cluster.Bounds.Posistion), cluster);
            }
        }
    }
}