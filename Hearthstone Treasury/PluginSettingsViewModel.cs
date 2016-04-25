﻿using Hearthstone_Deck_Tracker;
using System;
using System.IO;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hearthstone_Treasury
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

        public static PluginSettingsViewModel LoadSettings(string filePath)
        {
            PluginSettingsViewModel settings;
            if (File.Exists(filePath))
            {
                settings = XmlManager<PluginSettingsViewModel>.Load(filePath);
            }
            else
            {
                settings = new PluginSettingsViewModel()
                {
                    CollectionWindowWidth = 395,
                    CollectionWindowHeight = 560,
                    InitialBalance = 0
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