using System;
using PlatHak.Client.Common.Managers;
using PlatHak.Common.Enums;

namespace PlatHak.Client.Common.Interfaces
{
    public interface IPlatformManager
    {
        InputManager InputManager { get; }
        Device Device { get; }
        Guid SoftwareId { get; }
    }
}