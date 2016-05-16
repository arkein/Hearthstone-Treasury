using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class DistributionChartViewModel : ReactiveObject
    {
        private TransactionListViewModel _transactionsList;
        private Func<TransactionViewModel, bool> _selector;

        public DistributionChartViewModel(TransactionListViewModel transactionsList, Func<TransactionViewModel, bool> selector)
        {
            _selector = selector;
            SourcesList = new ReactiveList<IncomeSource>();

            _transactionsList = transactionsList;
            _transactionsList.TransactionsChanged.Subscribe(x => UpdateChart());

            UpdateChart();
        }

        private void UpdateChart()
        {
            using (SourcesList.SuppressChangeNotifications())
            {
                if (!SourcesList.IsEmpty)
                    SourcesList.Clear();

                foreach (var item in _transactionsList.Transactions.Where(_selector).GroupBy(t => t.Category).Select(t => new IncomeSource() { Name = Enum.GetName(t.Key.GetType(), t.Key), Amount = t.Sum(tt => Math.Abs(tt.Difference)) }))
                {
                    SourcesList.Add(item);
                }
            }
        }

        [Reactive]
        public ReactiveList<IncomeSource> SourcesList { get; set; }

        [Reactive]
        public string ChartTitle { get; set; }

        [Reactive]
        public string ChartSubTitle { get; set; }
    }

    public class IncomeSource : ReactiveObject
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public int Amount { get; set; }
    }
}
