﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Hearthstone_Treasury.ViewModels
{
    public class TransactionListViewModel : ReactiveObject
    {
        public TransactionListViewModel(ReactiveList<TransactionViewModel> transactionsList)
        {
            Transactions = transactionsList;
            TransactionsChanged.Select(_ => CalculateReportingDays()).ToProperty(this, x => x.ReportingDays, out reportingDays, CalculateReportingDays());
            TransactionsChanged.Select(_ => Transactions.Where(x => x.Difference > 0).Sum(x => x.Difference)).ToProperty(this, x => x.GoldIn, out goldIn, Transactions.Where(x => x.Difference > 0).Sum(x => x.Difference));
            TransactionsChanged.Select(_ => Transactions.Where(x => x.Difference < 0).Sum(x => x.Difference)).ToProperty(this, x => x.GoldOut, out goldOut, Transactions.Where(x => x.Difference < 0).Sum(x => x.Difference));
        }

        private double CalculateReportingDays()
        {
            double days = 1;
            if (Transactions.Any())
            {
                var daysDifference = Transactions.Max(t => t.Moment) - Transactions.Min(t => t.Moment);
                if (daysDifference.TotalDays > 0)
                {
                    days = daysDifference.TotalDays;
                }
            }
            return days;
        }

        [Reactive]
        public ReactiveList<TransactionViewModel> Transactions { get; private set; }

        private readonly ObservableAsPropertyHelper<double> reportingDays;
        public double ReportingDays { get { return reportingDays.Value; } }

        private readonly ObservableAsPropertyHelper<int> goldIn;
        public int GoldIn { get { return goldIn.Value; } }

        private readonly ObservableAsPropertyHelper<int> goldOut;
        public int GoldOut { get { return goldOut.Value; } }

        public IObservable<Unit> TransactionsChanged => Observable.Merge(Transactions.ItemChanged.Select(_ => Unit.Default), Transactions.Changed.Select(_ => Unit.Default));
    }
}