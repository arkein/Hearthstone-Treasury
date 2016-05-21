using Hearthstone_Treasury.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hearthstone_Treasury
{
    public class RachelleLogHandler
    {
        /// <summary>
        /// Copied form Hearthstone_Deck_Tracker.HsLogReaderConstants.GoldRewardRegex, extended with Origin info
        /// </summary>
        public static readonly Regex GoldRewardExtendedRegex = new Regex(@"GoldRewardData: Amount=(?<amount>(\d+)) Origin=(?<origin>(\w+)) OriginData=(?<origindata>(\d+))");

        private AchievementProvider _provider;
        private TransactionListViewModel _transactionList;

        public RachelleLogHandler(AchievementProvider provider, TransactionListViewModel list)
        {
            _provider = provider;
            _transactionList = list;
        }

        public void HandleRachelleLogUpdate(string logLine)
        {
            if (GoldRewardExtendedRegex.IsMatch(logLine))
            {
                // Only process new lines
                DateTime moment;
                if (IsLogLineOutdated(logLine, out moment))
                {
                    return;
                }

                var rewardInfo = CreateReward(logLine);

                _provider.ProvideComment(rewardInfo);

                var transaction = new TransactionViewModel { Difference = rewardInfo.Amount, Category = rewardInfo.Category, Comment = rewardInfo.Comment };

                if (moment > DateTime.MinValue)
                    transaction.Moment = moment;

                _transactionList.AddTransaction(transaction);
            }
        }

        public bool IsLogLineOutdated(string logLine, out DateTime moment)
        {
            moment = DateTime.MinValue;
            if (logLine.Length > 20 && DateTime.TryParse(logLine.Substring(2, 16), out moment))
            {
                if (moment > DateTime.Now)
                {
                    moment = moment.AddDays(-1);
                }

                var latestTransaction = _transactionList.Transactions.OrderByDescending(t => t.Moment).FirstOrDefault();
                if (latestTransaction != null)
                {
                    return moment < latestTransaction.Moment;
                }
            }
            return false;
        }

        public static GoldRewardViewModel CreateReward(string logLine)
        {
            var match = GoldRewardExtendedRegex.Match(logLine);
            return new GoldRewardViewModel(logLine, match.Groups["amount"].Value, match.Groups["origin"].Value, match.Groups["origindata"].Value);
        }
    }
}
