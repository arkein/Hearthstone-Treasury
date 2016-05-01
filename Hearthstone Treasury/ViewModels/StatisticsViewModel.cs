using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class StatisticsViewModel : ReactiveObject
    {
        public StatisticsViewModel(PluginSettingsViewModel settings, TransactionListViewModel transactionList)
        {
            TransactionList = transactionList;
            Settings = settings;

            Observable.Merge(Settings.WhenAnyValue(s => s.InitialBalance).Select(_ => Unit.Default), TransactionList.Changed.Select(_ => Unit.Default))
                .Select(_ => Settings.InitialBalance + TransactionList.Transactions.Sum(x => x.Difference))
                .ToPropertyEx(this, t => t.Balance, Settings.InitialBalance + TransactionList.Transactions.Sum(x => x.Difference));

            TransactionList.WhenAnyValue(t => t.GoldIn, t => t.ReportingDays)
                .Select(_ => TransactionList.GoldIn / TransactionList.ReportingDays)
                .ToPropertyEx(this, t => t.GoldIncomeVelocity, TransactionList.GoldIn / TransactionList.ReportingDays);

            TransactionList.WhenAnyValue(t => t.GoldOut, t => t.ReportingDays)
                .Select(_ => TransactionList.GoldOut / TransactionList.ReportingDays)
                .ToPropertyEx(this, t => t.GoldOutcomeVelocity, TransactionList.GoldOut / TransactionList.ReportingDays);

            this.WhenAnyValue(t => t.GoldIncomeVelocity, t => t.GoldOutcomeVelocity, (income, outcome) => income + outcome).ToPropertyEx(this, t => t.GoldTotalVelocity);
        }

        public extern int Balance { [ObservableAsProperty] get; }

        public extern double GoldIncomeVelocity { [ObservableAsProperty] get; }

        public extern double GoldOutcomeVelocity { [ObservableAsProperty] get; }

        public extern double GoldTotalVelocity { [ObservableAsProperty] get; }

        [Reactive]
        public PluginSettingsViewModel Settings { get; set; }

        [Reactive]
        public TransactionListViewModel TransactionList { get; set; }
    }
}
