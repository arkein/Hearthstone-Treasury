using OxyPlot;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class BalanceReportViewModel : ReactiveObject
    {
        private PluginSettingsViewModel _settings;

        private TransactionListViewModel _list;

        public BalanceReportViewModel(PluginSettingsViewModel settings, TransactionListViewModel list)
        {
            _settings = settings;
            _list = list;

            BalanceLineModel = new PlotModel()
            {
                Axes =
                {
                    new DateTimeAxis { IntervalType = DateTimeIntervalType.Days, StringFormat = "dd-MMM", Position = AxisPosition.Bottom },
                    new LinearAxis { Position = AxisPosition.Left }
                },
            };

            Observable.Merge(_settings.WhenAnyValue(s => s.InitialBalance).Select(_ => Unit.Default), _list.Changed.Select(_ => Unit.Default))
                .Subscribe(_ => RefreshModel(BuildSeries()));
        }

        private void RefreshModel(ISeries series)
        {
            BalanceLineModel.Series.Clear();
            BalanceLineModel.Series.Add(series);
            BalanceLineModel.UpdateData();
        }

        private ISeries BuildSeries()
        {
            var series = new LineSeries();

            int rollingBalance = _settings.InitialBalance;
            var groupedTransactions = _list.Transactions.GroupBy(t => t.Moment.Date).OrderBy(g => g.Key);

            foreach (var transactionsGroup in groupedTransactions)
            {
                if (series.Points == null || series.Points.Count == 0)
                {
                    series.Points.Add(DateTimeAxis.CreateDataPoint(transactionsGroup.Key.Subtract(TimeSpan.FromDays(1)), rollingBalance));
                }
                rollingBalance += transactionsGroup.Sum(t => t.Difference);
                series.Points.Add(DateTimeAxis.CreateDataPoint(transactionsGroup.Key, rollingBalance));
            }

            return series;
        }

        [Reactive]
        public PlotModel BalanceLineModel { get; set; }
    }
}
