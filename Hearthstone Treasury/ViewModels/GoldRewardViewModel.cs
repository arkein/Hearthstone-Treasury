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
            { 38, "Priest or Warlock Victory" },
            { 45, "Paladin or Priest Dominance" },
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
