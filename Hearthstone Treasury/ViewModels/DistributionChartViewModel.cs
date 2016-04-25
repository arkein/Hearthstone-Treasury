using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class DistributionChartViewModel : ReactiveObject
    {
        private IReactiveCollection<TransactionViewModel> _transactions;

        public DistributionChartViewModel(IReactiveCollection<TransactionViewModel> transactions)
        {
            _transactions = transactions;
            _transactions.ItemChanged.Subscribe(x => UpdateChart());
            _transactions.Changed.Subscribe(x => UpdateChart());

            SourcesList = new ReactiveList<IncomeSource>();

            UpdateChart();
        }

        private void UpdateChart()
        {
            SourcesList.Reset();
            SourcesList.AddRange(
                _transactions.Where(t => t.Difference > 0).GroupBy(t => t.Category).Select(t => new IncomeSource() { Name = Enum.GetName(t.Key.GetType(), t.Key), Amount = t.Sum(tt => tt.Difference) })
                );
        }

        [Reactive]
        public ReactiveList<IncomeSource> SourcesList { get; set; }

        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                // selected item has changed
                selectedItem = value;
            }
        }
    }

    public class IncomeSource : ReactiveObject
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public int Amount { get; set; }
    }
}
