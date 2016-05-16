using Hearthstone_Treasury.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Hearthstone_Treasury.ViewModels
{
    public class GoldRewardViewModel : ReactiveObject
    {
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
        }

        [Reactive]
        public string Source { get; set; }

        [Reactive]
        public int Amount { get; set; }

        [Reactive]
        public OriginEnum Origin { get; set; }

        [Reactive]
        public int? OriginData { get; set; }

        [Reactive]
        public string Comment { get; set; }

        [Reactive]
        public CategoryEnum Category { get; set; }
    }
}
