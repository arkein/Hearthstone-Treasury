using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hearthstone_Treasury.Utils;

namespace Tests
{
    [TestClass]
    public class AchieveParserTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var result = AchieveParser.Parse(@"Resources\ACHIEVE.xml");
        }
    }
}
