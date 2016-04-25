using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private PluginSettingsViewModel _settings;

        public MainWindowViewModel(PluginSettingsViewModel settings, ReactiveList<TransactionViewModel> transactionsList)
        {
            _settings = settings;
            Balance = _settings.InitialBalance;

            Transactions = transactionsList;
            Transactions.ItemChanged.Subscribe(x => UpdateBalance());
            Transactions.Changed.Subscribe(x => UpdateBalance());

            settings.WhenAnyValue(s => s.InitialBalance).Subscribe(x => UpdateBalance());

            DistributionChart = new DistributionChartViewModel(Transactions);
        }

        private void UpdateBalance()
        {
            int finalBalance = _settings.InitialBalance;
            if (Transactions.Any())
            {
                finalBalance += Transactions.Sum(x => x.Difference);
            }
            Balance = finalBalance; 

            double days = 1;
            if (Transactions.Any())
            {
                var daysDifference = Transactions.Max(t => t.Moment) - Transactions.Min(t => t.Moment);
                if (daysDifference.TotalDays > 0)
                {
                    days = daysDifference.TotalDays;
                }
            }
            var goldIncome = Transactions.Where(t => t.Difference > 0).Sum(t => t.Difference);
            var goldOutcome = Transactions.Where(t => t.Difference < 0).Sum(t => t.Difference);
            var goldTotal = Transactions.Sum(t => t.Difference);
            GoldVelocity = $"+{goldIncome / days:N2} | {goldOutcome / days:N2} | {(goldTotal > 0 ? "+" : "")}{goldTotal / days:N2}";
        }

        [Reactive]
        public ReactiveList<TransactionViewModel> Transactions { get; set; }

        [Reactive]
        public int Balance { get; set; }

        [Reactive]
        public PluginSettingsViewModel Settings { get; set; }

        [Reactive]
        public string GoldVelocity { get; set; }

        [Reactive]
        public DistributionChartViewModel DistributionChart { get; set; }
    }
}
