using Hearthstone_Deck_Tracker;
using System;
using System.IO;

namespace Hearthstone_Treasury
{
    [Serializable]
    public class PluginSettings
    {
        public double CollectionWindowWidth { get; set; }

        public double CollectionWindowHeight { get; set; }

        public int InitialBalance { get; set; }

        private const string STORAGE_FILE_NAME = "treasury.config.xml";

        public static PluginSettings LoadSettings(string dataDir)
        {
            string settingsFilePath = Path.Combine(dataDir, STORAGE_FILE_NAME);
            PluginSettings settings;
            if (File.Exists(settingsFilePath))
            {
                settings = XmlManager<PluginSettings>.Load(settingsFilePath);
            }
            else
            {
                settings = new PluginSettings()
                {
                    CollectionWindowWidth = 395,
                    CollectionWindowHeight = 560,
                    InitialBalance = 0
                };
            }

            return settings;
        }

        public void SaveSettings(string dataDir)
        {
            string settingsFilePath = Path.Combine(dataDir, STORAGE_FILE_NAME);
            XmlManager<PluginSettings>.Save(settingsFilePath, this);
        }
    }
}