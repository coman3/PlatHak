using System;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Managers;
using PlatHak.Client.Desktop.Properties;
using PlatHak.Common.Enums;

namespace PlatHak.Client.Desktop.Managers
{
    public class DesktopPlatformManager : IPlatformManager
    {
        private readonly WindowsInputManager _inputManager = new WindowsInputManager();
        public InputManager InputManager => _inputManager;
        public Device Device => Device.Desktop;

        public Guid SoftwareId => new Guid(Settings.Default.SoftwareId ?? (Settings.Default.SoftwareId = Guid.NewGuid().ToString()));


        public DesktopPlatformManager()
        {
        }
    }
}