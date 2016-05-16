using Hearthstone_Deck_Tracker;
using Hearthstone_Treasury.Models;
using Hearthstone_Treasury.Utils;
using Hearthstone_Treasury.ViewModels;
using System.IO;
using System.Linq;

namespace Hearthstone_Treasury
{
    public abstract class AchievementProvider
    {
        public static AchievementProvider Create(PluginSettingsViewModel settings, string parsedAchievementsPath, string hearthstoneAchievementsFile)
        {
            if (string.IsNullOrEmpty(hearthstoneAchievementsFile) || !File.Exists(hearthstoneAchievementsFile))
            {
                return new HardcodedAchievementProvider();
            }

            AchievementDbf parsedAchievements;

            if (string.IsNullOrEmpty(parsedAchievementsPath) || !File.Exists(parsedAchievementsPath))
            {
                parsedAchievements = AchieveParser.Parse(hearthstoneAchievementsFile);
                XmlManager<AchievementDbf>.Save(parsedAchievementsPath, parsedAchievements);

                return new DbfAchievementProvider(settings, parsedAchievements);
            }

            parsedAchievements = XmlManager<AchievementDbf>.Load(parsedAchievementsPath);

            if (parsedAchievements.LastModified >= File.GetLastWriteTimeUtc(hearthstoneAchievementsFile))
            {
                return new DbfAchievementProvider(settings, parsedAchievements);
            }

            if (parsedAchievements.Fingerprint != AchieveParser.ReadFingerprint(hearthstoneAchievementsFile))
            {
                parsedAchievements = AchieveParser.Parse(hearthstoneAchievementsFile);
                XmlManager<AchievementDbf>.Save(parsedAchievementsPath, parsedAchievements);

                return new DbfAchievementProvider(settings, parsedAchievements);
            }
            else
            {
                File.SetLastWriteTimeUtc(parsedAchievementsPath, File.GetLastWriteTimeUtc(hearthstoneAchievementsFile));
                return new DbfAchievementProvider(settings, parsedAchievements);
            }
        }

        public abstract void ProvideComment(GoldRewardViewModel rewardInfo);
    }

    public class DbfAchievementProvider : AchievementProvider
    {
        private readonly AchievementDbf _knownAchievements;
        private readonly PluginSettingsViewModel _settings;

        public DbfAchievementProvider(PluginSettingsViewModel settings, AchievementDbf achievements)
        {
            _settings = settings;
            _knownAchievements = achievements;
        }

        public override void ProvideComment(GoldRewardViewModel rewardInfo)
        {
            if (rewardInfo.OriginData.HasValue) {
                var achievement = _knownAchievements.Records.FirstOrDefault(a => a.Enabled && a.Reward == Enums.RewardType.Gold && a.Id == rewardInfo.OriginData);
                if (achievement != null)
                {
                    rewardInfo.Comment = achievement.GetName(_settings.Locale);
                    return;
                }
            }
            rewardInfo.Comment = "*";
        }
    }

    public class HardcodedAchievementProvider : AchievementProvider
    {
        public override void ProvideComment(GoldRewardViewModel rewardInfo)
        {
            //map origindata to known comment
            string comment;
            if (rewardInfo.OriginData.HasValue && HardcodedAchievements.AchievementIdToComment.TryGetValue(rewardInfo.OriginData.Value, out comment))
            {
                rewardInfo.Comment = comment;
            }
            else
            {
                rewardInfo.Comment = "*";
            }
        }
    }
}
