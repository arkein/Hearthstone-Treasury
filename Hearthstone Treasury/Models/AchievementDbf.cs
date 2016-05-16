using System;
using System.Collections.Generic;

namespace Hearthstone_Treasury.Models
{
    [Serializable]
    public class AchievementDbf
    {
        public AchievementDbf()
        {
            Records = new List<Achievement>();
        }

        //<SourceFingerprint>mPk+Ctv3dsDTtiWY0+6mp+eL414=</SourceFingerprint>
        public string Fingerprint { get; set; }

        public DateTime LastModified { get; set; }

        public List<Achievement> Records { get; set; }
    }
}
