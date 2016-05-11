using Hearthstone_Treasury.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;

namespace Hearthstone_Treasury.ViewModels
{
    public class GoldRewardViewModel : ReactiveObject
    {
        /// <summary>
        /// Mapping between Achievement ID which is basically quest ID and quest text. This mapping should be removed when achievement log parser is implemented in HDT.
        /// </summary>
        internal readonly Dictionary<int, string> AchievementIdToComment = new Dictionary<int, string>
        {
            //Combined
            { 32, "Druid or Hunter Victory"},
            { 33, "Druid or Rogue Victory"},
            { 34, "Hunter or Mage Victory"},//confirmed
            { 35, "Mage or Shaman Victory"},
            { 36, "Paladin or Priest Victory"},
            { 37, "Paladin or Warrior Victory"},
            { 38, "Priest or Warlock Victory"},//confirmed
            { 39, "Rogue or Warrior Victory"},
            { 40, "Shaman or Warlock Victory"},//confirmed
            
            //Combined Dominance
            { 41, "Druid or Hunter Dominance"},
            { 42, "Druid or Rogue Dominance"},
            { 43, "Hunter or Mage Dominance"},
            { 44, "Mage or Shaman Dominance"},
            { 45, "Paladin or Priest Dominance"},//confirmed
            { 46, "Paladin or Warrior Dominance"},
            { 47, "Priest or Warlock Dominance"},
            { 48, "Rogue or Warrior Dominance"},
            { 49, "Shaman or Warlock Dominance"},

            //Generic
            { 52, "The Meek Shall Inherit" },//confirmed
            { 53, "Spell Master" },//confirmed
            { 69, "3 Victories!" },//confirmed

            //Class
             { 220, "Druid Victory"},
             { 221, "Hunter Victory"},
             { 222, "Rogue Victory"},
             { 223, "Mage Victory"},
             { 224, "Shaman Victory"},
             { 225, "Priest Victory"},
             { 226, "Paladin Victory"},//confirmed
             { 227, "Warrior Victory"},
             { 228, "Warlock Victory"},

            /* Total -35, interested - 14
             * Generic
             * { ?, "Destroy them All"},
             * { ?, "Only the Mighty"},
             * { 52, "The Meek Shall Inherit" },
             * { 53, "Spell Master" },
             * { ?, "Beat Down"},
             * { ?, "Total Dominance"},
             * { 69, "3 Victories!" },
             * { ?, "Everybody! Get in here!"},
             * 
             * Class
             * { ?, "Druid Victory"},
             * { ?, "Hunter Victory"},
             * { ?, "Rogue Victory"},
             * { ?, "Mage Victory"},
             * { ?, "Shaman Victory"},
             * { ?, "Priest Victory"},
             * { 226, "Paladin Victory"},
             * { ?, "Warrior Victory"},
             * { ?, "Warlock Victory"},
             * 
             * Class Dominance - does it exist?
             * { ?, "Druid Dominance"},
             * { ?, "Hunter Dominance"},
             * { ?, "Rogue Dominance"},
             * { ?, "Mage Dominance"},
             * { ?, "Shaman Dominance"},
             * { ?, "Priest Dominance"},
             * { ?, "Paladin Dominance"},
             * { ?, "Warrior Dominance"},
             * { ?, "Warlock Dominance"},
             * 
             * Combined
             * { ?, "Druid or Hunter Victory"},
             * { ?, "Druid or Rogue Victory"},
             * { 34, "Hunter or Mage Victory"},
             * { ?, "Mage or Shaman Victory"},
             * { ?, "Paladin or Priest Victory"},
             * { ?, "Paladin or Warrior Victory"},
             * { 38, "Priest or Warlock Victory"},
             * { ?, "Rogue or Warrior Victory"},
             * { 40, "Shaman or Warlock Victory"},
             * 
             * Combined Dominance
             * { ?, "Druid or Hunter Dominance"},
             * { ?, "Druid or Rogue Dominance"},
             * { ?, "Hunter or Mage Dominance"},
             * { ?, "Mage or Shaman Dominance"},
             * { 45, "Paladin or Priest Dominance"},
             * { ?, "Paladin or Warrior Dominance"},
             * { ?, "Priest or Warlock Dominance"},
             * { ?, "Rogue or Warrior Dominance"},
             * { ?, "Shaman or Warlock Dominance"},
             * 
             * Special
             * { ?, "The Duelist"},
             * { ?, "Chicken Dinner"},
             * { ?, "Big Winner"},
             * { ?, "Ready to Go!"},
             * { ?, "Crushed Them All!"},
             * { ?, "Got the Basics!"},
             * { ?, "One of Everything!"},
             */
        };

        public GoldRewardViewModel(string sourceString, string amountString, string originString, string originDataString)
        {
            Source = sourceString;

            int amount;
            if(int.TryParse(amountString, out amount))
            {
                Amount = amount;
            }

            int originData;
            if (int.TryParse(originDataString, out originData))
            {
                OriginData = originData;
            } else
            {
                OriginData = null;
            }

            OriginEnum origin;
            if (Enum.TryParse(originString, out origin))
            {
                Origin = origin;
            } else
            {
                Origin = OriginEnum.UNKNOWN;
            }

            //map origin to category
            switch (Origin)
            {
                case OriginEnum.TOURNEY:
                    Category = CategoryEnum.Wins;
                    break;
                case OriginEnum.ACHIEVEMENT:
                    Category = CategoryEnum.Daily;
                    break;
                default:
                    Category = CategoryEnum.Other;
                    break;
            }

            //map origindata to known comment
            string comment;
            if (OriginData.HasValue && AchievementIdToComment.TryGetValue(OriginData.Value, out comment))
            {
                Comment = comment;
            } else
            {
                Comment = "*";
            }
        }

        [Reactive]
        public string Source { get; private set; }

        [Reactive]
        public int Amount { get; private set; }

        [Reactive]
        public OriginEnum Origin { get; private set; }

        [Reactive]
        public int? OriginData { get; private set; }

        [Reactive]
        public string Comment { get; private set; }

        [Reactive]
        public CategoryEnum Category { get; private set; }
    }
}
