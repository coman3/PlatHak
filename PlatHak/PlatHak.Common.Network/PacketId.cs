using System;

namespace PlatHak.Common.Network
{
    [Serializable]
    public enum PacketId : byte
    {
        Handshake,
        Error,
        Event,
        Login,

        World,
        UpdateGridItem,
        Player,
        Chunk,
        PlayerMove,
        MoveRequest,
        ChunkRequest
    }
}