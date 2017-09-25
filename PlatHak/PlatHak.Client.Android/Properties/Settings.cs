using System;
using System.Text;
using Newtonsoft.Json;
using PlatHak.Common.Helpers;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PlatHak.Client.Android.Properties
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string SettingsKey = "6dba84b2-9344-4f93-bb8f-4458e14bcac3";
        private static readonly string SettingsDefault = JsonConvert.SerializeObject(new AndroidSettings(true)).ToBase64();

        #endregion


        public static AndroidSettings AllSettings
        {
            get => JsonConvert.DeserializeObject<AndroidSettings>(AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault).FromBase64());
            set => AppSettings.AddOrUpdateValue(SettingsKey, JsonConvert.SerializeObject(value).ToBase64());
        }

    }

    public class AndroidSettings
    {
        public Guid SoftwareId { get; set; }

        public AndroidSettings(bool create = false)
        {
            if (create)
            {
                SoftwareId = Guid.NewGuid();
            }
        }
    }
}
