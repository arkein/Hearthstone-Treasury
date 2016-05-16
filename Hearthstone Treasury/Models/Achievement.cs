using Hearthstone_Treasury.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hearthstone_Treasury.Models
{
    [Serializable]
    public class Achievement
    {
        public Achievement()
        {
            LocalizedName = new List<LocalizedNamePair>();
        }

        public int Id { get; set; }

        public bool Enabled { get; set; }

        public RewardType Reward { get; set; }

        public int RewardData1 { get; set; }

        public int RewardData2 { get; set; }

        public string DefaultName { get; set; }

        public List<LocalizedNamePair> LocalizedName { get; set; }

        public string GetName(string culture)
        {
            return LocalizedName.FirstOrDefault(t=>t.Lang==culture)?.Name ?? DefaultName;
        }
    }

    public class LocalizedNamePair
    {
        public string Lang;
        public string Name;
    }
}
