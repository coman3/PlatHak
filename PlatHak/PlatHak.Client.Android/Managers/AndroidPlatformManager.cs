using System;
using PlatHak.Client.Android.Properties;
using PlatHak.Client.Common.Interfaces;
using PlatHak.Client.Common.Managers;
using PlatHak.Common.Enums;

namespace PlatHak.Client.Android.Managers
{
    public class AndroidPlatformManager : IPlatformManager
    {
        private readonly AndroidInputManager _androidInputManager = new AndroidInputManager();

        public InputManager InputManager => _androidInputManager;
        public Device Device => Device.Android;
        public Guid SoftwareId => Settings.AllSettings.SoftwareId;

    }
}