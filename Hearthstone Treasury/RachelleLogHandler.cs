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

                if (!IsDuplicate(transaction))
                {
                    _transactionList.AddTransaction(transaction);
                }
            }
        }

        /// <summary>
        /// Prevents completely duplicated transactions which are sometimes invoked by HDT rachelle log handler.
        /// </summary>
        /// <param name="transaction">Transaction to check for duplication</param>
        /// <returns>true if already in the list</returns>
        public bool IsDuplicate(TransactionViewModel transaction)
        {
            return _transactionList.Transactions.Any(t =>
                                        t.Moment == transaction.Moment &&
                                        t.Difference == transaction.Difference &&
                                        t.Category == transaction.Category &&
                                        t.Comment == transaction.Comment);
        }

        public bool IsLogLineOutdated(string logLine, out DateTime moment)
        {
            if (LogStringToDateTime(logLine, out moment))
            {
                var latestTransaction = _transactionList.Transactions.OrderByDescending(t => t.Moment).FirstOrDefault();

                if (latestTransaction == null || moment < latestTransaction.Moment)
                {
                    return false;
                }
            }

            return false;
        }

        public static bool LogStringToDateTime(string logLine, out DateTime moment)
        {
            moment = DateTime.MinValue;
            if (logLine.Length <= 20 || !DateTime.TryParse(logLine.Substring(2, 16), out moment))
            {
                return false;
            }

            if (moment > DateTime.Now)
            {
                moment = moment.AddDays(-1);
            }

            return true;
        }

        public static GoldRewardViewModel CreateReward(string logLine)
        {
            var match = GoldRewardExtendedRegex.Match(logLine);
            return new GoldRewardViewModel(logLine, match.Groups["amount"].Value, match.Groups["origin"].Value, match.Groups["origindata"].Value);
        }
    }
}
