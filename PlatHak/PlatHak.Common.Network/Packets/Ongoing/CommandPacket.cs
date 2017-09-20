using System;
using PlatHak.Common.Maths;

namespace PlatHak.Common.Network
{
    public class CommandPacket : Packet
    {
        public Command Command { get; set; }
        public CommandPacket(Command command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }
    }

    public abstract class Command
    {
        
    }

    public class PlayerCommand : Command
    {

    }

    public class MovePlayerCommand : PlayerCommand
    {
        public Vector2 Direction { get; set; }

    }
}