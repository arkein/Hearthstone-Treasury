using Hearthstone_Treasury.Enums;
using Hearthstone_Treasury.Models;
using System;
using System.IO;
using System.Xml;

namespace Hearthstone_Treasury.Utils
{
    public static class AchieveParser
    {
        public static AchievementDbf Parse(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            var achievements = new AchievementDbf();
            Achievement achievement;

            using (var stream = File.OpenRead(path))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    if (reader.ReadToFollowing("Dbf"))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name.ToString())
                                {
                                    case "SourceFingerprint":
                                        achievements.Fingerprint = reader.ReadElementContentAsString();
                                        break;
                                    case "Record":
                                        using (var subtree = reader.ReadSubtree())
                                        {
                                            if (!subtree.ReadToDescendant("Field"))
                                            {
                                                break;
                                            }
                                            achievement = new Achievement();
                                            ReadFieldInto(achievement, subtree);

                                            while (subtree.ReadToNextSibling("Field"))
                                            {
                                                ReadFieldInto(achievement, subtree);
                                            }
                                            achievements.Records.Add(achievement);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            achievements.LastModified = File.GetLastWriteTimeUtc(path);

            return achievements;
        }

        private static void ReadFieldInto(Achievement achievement, XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                return;
            }

            var currentFieldName = reader.GetAttribute("column");

            switch (currentFieldName)
            {
                case "ID":
                    achievement.Id = int.Parse(reader.ReadElementContentAsString("Field", string.Empty));
                    break;
                case "NOTE_DESC":
                    achievement.DefaultName = reader.ReadElementContentAsString("Field", string.Empty);
                    break;
                case "ENABLED":
                    achievement.Enabled = bool.Parse(reader.ReadElementContentAsString("Field", string.Empty));
                    break;
                case "REWARD":
                    RewardType type;
                    if (Enum.TryParse(reader.ReadElementContentAsString("Field", string.Empty), true, out type))
                    {
                        achievement.Reward = type;
                    }
                    else
                    {
                        achievement.Reward = RewardType.Other;
                    }
                    break;
                case "REWARD_DATA1":
                    achievement.RewardData1 = int.Parse(reader.ReadElementContentAsString("Field", string.Empty));
                    break;
                case "REWARD_DATA2":
                    achievement.RewardData2 = int.Parse(reader.ReadElementContentAsString("Field", string.Empty));
                    break;
                case "NAME":
                    using (var subreader = reader.ReadSubtree())
                    {
                        subreader.ReadStartElement();
                        while (subreader.Read())
                        {
                            if (subreader.IsStartElement())
                            {
                                var namePair = new LocalizedNamePair { Lang = subreader.Name, Name = subreader.ReadElementContentAsString() };
                                string correctLocale;
                                if (Localization.BlizzardToWindowsLocale.TryGetValue(namePair.Lang, out correctLocale))
                                {
                                    namePair.Lang = correctLocale;
                                }
                                else
                                {

                                }
                                achievement.LocalizedName.Add(namePair);
                            }
                        }
                    }
                    break;
            }
        }

        public static string ReadFingerprint(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (var stream = File.OpenRead(path))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    if (reader.ReadToFollowing("SourceFingerprint"))
                    {
                        return reader.ReadElementContentAsString();
                    }
                }
            }

            return string.Empty;
        }
    }
}
