using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel(PluginSettingsViewModel settings, TransactionListViewModel transactionsList)
        {
            Settings = settings;
            TransactionList = transactionsList;
            Reports = new ReportControlViewModel(TransactionList);
            BalanceReport = new BalanceReportViewModel(settings, TransactionList);
            Statistics = new StatisticsViewModel(settings, transactionsList);

            OptionsFlyoutOpen = ReactiveCommand.Create(this.WhenAnyValue(x => x.OptionsFlyoutState, state => !state));
            OptionsFlyoutOpen.Subscribe(_ => OptionsFlyoutState = !OptionsFlyoutState);
        }

        public ReactiveCommand<object> OptionsFlyoutOpen { get; private set; }

        [Reactive]
        public bool OptionsFlyoutState { get; set; }

        [Reactive]
        public TransactionListViewModel TransactionList { get; private set; }

        [Reactive]
        public ReportControlViewModel Reports { get; private set; }

        [Reactive]
        public BalanceReportViewModel BalanceReport { get; private set; }

        [Reactive]
        public StatisticsViewModel Statistics { get; private set; }

        [Reactive]
        public PluginSettingsViewModel Settings { get; set; }
    }
}
