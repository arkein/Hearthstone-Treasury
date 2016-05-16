using Hearthstone_Deck_Tracker;
using System;
using System.IO;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hearthstone_Treasury.ViewModels
{
    [Serializable]
    public class PluginSettingsViewModel : ReactiveObject
    {
        [Reactive]
        public double CollectionWindowWidth { get; set; }

        [Reactive]
        public double CollectionWindowHeight { get; set; }

        [Reactive]
        public int InitialBalance { get; set; }

        [Reactive]
        public string Locale { get; set; }

        public static PluginSettingsViewModel LoadSettings(string filePath)
        {
            PluginSettingsViewModel settings;
            if (File.Exists(filePath))
            {
                settings = XmlManager<PluginSettingsViewModel>.Load(filePath);
                if (string.IsNullOrEmpty(settings.Locale))
                {
                    settings.Locale = System.Globalization.CultureInfo.CurrentUICulture.Name;
                }
            }
            else
            {
                settings = new PluginSettingsViewModel()
                {
                    CollectionWindowWidth = 487,
                    CollectionWindowHeight = 666,
                    InitialBalance = 0,
                    Locale = System.Globalization.CultureInfo.CurrentUICulture.Name
                };
            }

            return settings;
        }

        public void SaveSettings(string filePath)
        {
            XmlManager<PluginSettingsViewModel>.Save(filePath, this);
        }
    }
}