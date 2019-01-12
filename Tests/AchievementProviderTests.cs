using Hearthstone_Treasury;
using Hearthstone_Treasury.ViewModels;
using System.IO;
using NUnit.Framework;
using System;
using ReactiveUI;

namespace Tests
{
    [TestFixture]
    public class AchievementProviderTests
    {
        private static PluginSettingsViewModel _settings = new PluginSettingsViewModel { Locale = "en-US" };
        private static string myFile = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\achievements_parsed.xml";
        private static string blizzFile = AppDomain.CurrentDomain.BaseDirectory+@"\Resources\achieve.xml";

        public static AchievementProvider CreateHardcodedProvider()
        {
            return AchievementProvider.Create(_settings, "", "");
        }

        public static AchievementProvider CreateDbfProvider()
        {
            return AchievementProvider.Create(_settings, myFile, blizzFile);
        }

        [Test]
        public void CreateHardcodedProviderTest()
        {
            var provider = CreateHardcodedProvider();

            Assert.That(Equals(provider.GetType(), typeof(HardcodedAchievementProvider)));
        }

        [Test]
        public void CreateDbfProviderTest()
        {
            var provider = CreateDbfProvider();

            Assert.That(Equals(provider.GetType(), typeof(DbfAchievementProvider)));
            Assert.That(File.Exists(myFile), Is.True);
        }

        [Test]
        public void IsOutdatedLineTest() {
            var loglines = new string[] {
                @"D 13:12:35.1745081 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=10 Origin=TOURNEY OriginData=0]",
                @"D 13:12:39.8007727 EndGameTwoScoop.UpdateData(): 3/3 wins towards 10 gold",
                @"D 13:17:25.1560941 NetCache.OnProfileNotices() sending 2 new notices to 7 listeners",
                @"D 13:17:25.1880959 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=40 Origin=ACHIEVEMENT OriginData=37]",
                @"D 13:17:25.1880959 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=60 Origin=ACHIEVEMENT OriginData=48]",
                @"D 13:17:29.7293557 EndGameTwoScoop.UpdateData(): 1/3 wins towards 10 gold",
            };
            var provider = CreateDbfProvider();
            var transactionList = new TransactionListViewModel(new ReactiveList<TransactionViewModel>());
            var handler = new GameplayLogHandler(provider, transactionList);

            foreach (var logline in loglines)
            {
                handler.HandleGameplayLogUpdate(logline);
            }

            Assert.That(transactionList.Transactions.Count, Is.EqualTo(3));

            var transaction = transactionList.Transactions[0];
            Assert.That(transaction.Category, Is.EqualTo(Hearthstone_Treasury.Enums.CategoryEnum.Wins));
            Assert.That(transaction.Difference, Is.EqualTo(10));
            Assert.That(transaction.Comment, Is.EqualTo("*"));
            Assert.That(transaction.Moment.TimeOfDay, Is.EqualTo(new TimeSpan(0, 13, 12, 35, 174).Add(new TimeSpan(5081))));

            transaction = transactionList.Transactions[1];
            Assert.That(transaction.Category, Is.EqualTo(Hearthstone_Treasury.Enums.CategoryEnum.Daily));
            Assert.That(transaction.Difference, Is.EqualTo(40));
            Assert.That(transaction.Comment, Is.EqualTo("Paladin or Warrior Victory"));
            Assert.That(transaction.Moment.TimeOfDay, Is.EqualTo(new TimeSpan(0, 13, 17, 25, 188).Add(new TimeSpan(959))));

            transaction = transactionList.Transactions[2];
            Assert.That(transaction.Category, Is.EqualTo(Hearthstone_Treasury.Enums.CategoryEnum.Daily));
            Assert.That(transaction.Difference, Is.EqualTo(60));
            Assert.That(transaction.Comment, Is.EqualTo("Rogue or Warrior Dominance"));
            Assert.That(transaction.Moment.TimeOfDay, Is.EqualTo(new TimeSpan(0, 13, 17, 25, 188).Add(new TimeSpan(959))));
        }

        [Test]
        public void DuplicateLinesTest()
        {
            var loglines = new string[] {
                @"D 13:12:35.1745081 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=10 Origin=TOURNEY OriginData=0]",
                @"D 13:12:39.8007727 EndGameTwoScoop.UpdateData(): 3/3 wins towards 10 gold",
                @"D 13:17:25.1560941 NetCache.OnProfileNotices() sending 2 new notices to 7 listeners",
                @"D 13:17:25.1880959 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=40 Origin=ACHIEVEMENT OriginData=37]",
                @"D 13:17:25.1880959 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=60 Origin=ACHIEVEMENT OriginData=48]",
                @"D 13:17:29.7293557 EndGameTwoScoop.UpdateData(): 1/3 wins towards 10 gold",
            };
            var provider = CreateDbfProvider();
            var transactionList = new TransactionListViewModel(new ReactiveList<TransactionViewModel>());
            var handler = new GameplayLogHandler(provider, transactionList);

            foreach (var logline in loglines)
            {
                handler.HandleGameplayLogUpdate(logline);
            }

            Assert.That(transactionList.Transactions.Count, Is.EqualTo(3));

            foreach (var logline in loglines)
            {
                handler.HandleGameplayLogUpdate(logline);
            }

            Assert.That(transactionList.Transactions.Count, Is.EqualTo(3));

            var transaction = transactionList.Transactions[0];
            Assert.That(transaction.Category, Is.EqualTo(Hearthstone_Treasury.Enums.CategoryEnum.Wins));
            Assert.That(transaction.Difference, Is.EqualTo(10));
            Assert.That(transaction.Comment, Is.EqualTo("*"));
            Assert.That(transaction.Moment.TimeOfDay, Is.EqualTo(new TimeSpan(0, 13, 12, 35, 174).Add(new TimeSpan(5081))));

            transaction = transactionList.Transactions[1];
            Assert.That(transaction.Category, Is.EqualTo(Hearthstone_Treasury.Enums.CategoryEnum.Daily));
            Assert.That(transaction.Difference, Is.EqualTo(40));
            Assert.That(transaction.Comment, Is.EqualTo("Paladin or Warrior Victory"));
            Assert.That(transaction.Moment.TimeOfDay, Is.EqualTo(new TimeSpan(0, 13, 17, 25, 188).Add(new TimeSpan(959))));

            transaction = transactionList.Transactions[2];
            Assert.That(transaction.Category, Is.EqualTo(Hearthstone_Treasury.Enums.CategoryEnum.Daily));
            Assert.That(transaction.Difference, Is.EqualTo(60));
            Assert.That(transaction.Comment, Is.EqualTo("Rogue or Warrior Dominance"));
            Assert.That(transaction.Moment.TimeOfDay, Is.EqualTo(new TimeSpan(0, 13, 17, 25, 188).Add(new TimeSpan(959))));
        }

        [Test]
        public void GoldRewardExtendedRegexTest() {
            string singleLogLine = @"D 23:12:35.5158504 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=40 Origin=ACHIEVEMENT OriginData=39]";
            var reward = GameplayLogHandler.CreateReward(singleLogLine);

            Assert.That(reward.Amount == 40);
            Assert.That(reward.Origin == Hearthstone_Treasury.Enums.OriginEnum.ACHIEVEMENT);
            Assert.That(reward.OriginData == 39);
        }

        [Test]
        public void ProvideCommentTest()
        {
            var provider = CreateDbfProvider();
            Assert.That(Equals(provider.GetType(), typeof(DbfAchievementProvider)));

            string singleLogLine = @"D 23:12:35.5158504 RewardUtils.GetViewableRewards() - processing reward [GoldRewardData: Amount=40 Origin=ACHIEVEMENT OriginData=39]";

            var reward = GameplayLogHandler.CreateReward(singleLogLine);

            provider.ProvideComment(reward);

            Assert.That(reward.Comment, Is.EqualTo("Rogue or Warrior Victory"));
        }

    }
}
