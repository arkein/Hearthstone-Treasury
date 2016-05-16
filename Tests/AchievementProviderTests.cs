using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hearthstone_Treasury;
using Hearthstone_Treasury.ViewModels;
using System.IO;

namespace Tests
{
    [TestClass]
    public class AchievementProviderTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var settings = new PluginSettingsViewModel { Locale = "ru-RU" };
            var myFile = Path.GetFullPath(@"Resources\achievements_parsed.xml");
            var blizzFile = Path.GetFullPath(@"Resources\achieve.xml");
            var provider = AchievementProvider.Create(settings, myFile , blizzFile);
        }
    }
}
