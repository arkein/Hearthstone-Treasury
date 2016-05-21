using Hearthstone_Deck_Tracker;
using Hearthstone_Treasury.ViewModels;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class SettingsTests
    {
        private static string settingsFile = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\treasury.test.config.xml";

        [Test]
        public void LoadSettingsTest()
        {
            var settings = XmlManager<PluginSettingsViewModel>.Load(settingsFile);
        }
    }
}
