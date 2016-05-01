using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hearthstone_Treasury.ViewModels
{
    public class ReportControlViewModel: ReactiveObject
    {
        public ReportControlViewModel(TransactionListViewModel transactionsList)
        {
            IncomeDistributionChart = new DistributionChartViewModel(transactionsList, t => t.Difference > 0) { ChartTitle = "Gold income by source", ChartSubTitle = "Shows where your gold comes from" };
            OutcomeDistributionChart = new DistributionChartViewModel(transactionsList, t => t.Difference < 0) { ChartTitle = "Gold outcome by source", ChartSubTitle = "Shows your spendings" };
        }

        [Reactive]
        public DistributionChartViewModel IncomeDistributionChart { get; set; }

        [Reactive]
        public DistributionChartViewModel OutcomeDistributionChart { get; set; }
    }
}
