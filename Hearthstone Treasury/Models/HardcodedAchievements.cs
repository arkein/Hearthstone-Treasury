using System.Collections.Generic;

namespace Hearthstone_Treasury.Models
{
    public static class HardcodedAchievements
    {
        public static readonly Dictionary<int, string> AchievementIdToComment = new Dictionary<int, string>
        {
            //Combined
            { 32, "Druid or Hunter Victory"},
            { 33, "Druid or Rogue Victory"},
            { 34, "Hunter or Mage Victory"},
            { 35, "Mage or Shaman Victory"},
            { 36, "Paladin or Priest Victory"},
            { 37, "Paladin or Warrior Victory"},
            { 38, "Priest or Warlock Victory"},
            { 39, "Rogue or Warrior Victory"},
            { 40, "Shaman or Warlock Victory"},
            
            //Combined Dominance
            { 41, "Druid or Hunter Dominance"},
            { 42, "Druid or Rogue Dominance"},
            { 43, "Hunter or Mage Dominance"},
            { 44, "Mage or Shaman Dominance"},
            { 45, "Paladin or Priest Dominance"},
            { 46, "Paladin or Warrior Dominance"},
            { 47, "Priest or Warlock Dominance"},
            { 48, "Rogue or Warrior Dominance"},
            { 49, "Shaman or Warlock Dominance"},

            //Generic
            { 50, "Destroy them All"},
            { 51, "Only the Mighty"},
            { 52, "The Meek Shall Inherit" },
            { 53, "Spell Master" },
            { 54, "Beat Down"},
            { 64, "Total Dominance"},
            { 69, "3 Victories!" },
            { 222, "Everybody! Get in here!"},

            // Special
            { 12, "The Duelist"},
            { 62, "Chicken Dinner"},
            { 63, "Big Winner"},
            { 79, "Ready to Go!"},
            { 89, "Crushed Them All!"},
            { 90, "Got the Basics!"},
            { 91, "One of Everything!"},

            //Class
             { 223, "Druid Victory"},
             { 224, "Hunter Victory"},
             { 225, "Mage Victory"},
             { 226, "Paladin Victory"},
             { 227, "Priest Victory"},
             { 228, "Rogue Victory"},
             { 229, "Shaman Victory"},
             { 230, "Warlock Victory"},
             { 231, "Warrior Victory"},
        };
    }
}
