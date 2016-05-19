﻿using Hearthstone_Treasury.Controls;
using Hearthstone_Treasury.ViewModels;
using System;
using System.IO;
using System.Windows;
using ReactiveUI;
using System.Text.RegularExpressions;
using System.Linq;
using Hearthstone_Deck_Tracker;

namespace Hearthstone_Treasury
{
    public class HearthstoneTreasuryPlugin : Hearthstone_Deck_Tracker.Plugins.IPlugin
    {
        /// <summary>
        /// Copied form Hearthstone_Deck_Tracker.HsLogReaderConstants.GoldRewardRegex, extended with Origin info
        /// </summary>
        public static readonly Regex GoldRewardExtendedRegex = new Regex(@"GoldRewardData: Amount=(?<amount>(\d+)) Origin=(?<origin>(\w+)) OriginData=(?<origindata>(\d+))");

        internal static string PluginDataDir => Path.Combine(Config.Instance.DataDir, "Treasury");
        internal static string TransactionsFile => Path.Combine(PluginDataDir, "transactions.xml");
        internal static string SettingsFile => Path.Combine(PluginDataDir, "treasury.config.xml");
        internal static string AchievementsFile => Path.Combine(PluginDataDir, "achievements.xml");

        internal const string AchievementDbfFile = @"\DBF\ACHIEVE.xml";
        internal static readonly string AchievementDbfFilePath = Config.Instance.HearthstoneDirectory + AchievementDbfFile;

        private PluginSettingsViewModel Settings { get; set; }

        private readonly TreasuryMenuItem _menuItem = new TreasuryMenuItem { Header = "Treasury" };

        public System.Windows.Controls.MenuItem MenuItem => _menuItem;

        private MainWindowViewModel _mainWindowModel;

        private MainWindow _mainWindow;

        private AchievementProvider _achievementProvider;

        public string Author => "Arkein";

        public string ButtonText => "Settings are inside plugin";

        public string Description => "Gold tracking plugin for Hearthstone Deck Tracker.";

        public string Name => "Treasury";

        public Version Version => new Version(0, 0, 0, 1);

        public void OnButtonPress()
        {
            //no-op
        }

        public void OnLoad()
        {
            if (!Directory.Exists(PluginDataDir))
            {
                Directory.CreateDirectory(PluginDataDir);
            }

            Settings = PluginSettingsViewModel.LoadSettings(SettingsFile);

            var pathToHearthstoneAchievementsFile = string.IsNullOrEmpty(Config.Instance.HearthstoneDirectory) ? "" : AchievementDbfFilePath;
            _achievementProvider = AchievementProvider.Create(Settings, AchievementsFile, pathToHearthstoneAchievementsFile);

            var transactions = TransactionHelper.LoadTransactions(TransactionsFile) ?? new ReactiveList<TransactionViewModel>() { ChangeTrackingEnabled = true };
            var transactionList = new TransactionListViewModel(transactions);

            _mainWindowModel = new MainWindowViewModel(Settings, transactionList);

            _menuItem.Click += (sender, args) =>
            {
                if (_mainWindow == null)
                {
                    InitializeMainWindow();
                    _mainWindow.Show();
                }
                else
                {
                    _mainWindow.Activate();
                }
            };

            Hearthstone_Deck_Tracker.API.LogEvents.OnRachelleLogLine.Add(HandleRachelleLogUpdate);
        }

        private void HandleRachelleLogUpdate(string logLine)
        {
            if (GoldRewardExtendedRegex.IsMatch(logLine))
            {
                // Only process new lines
                if (IsLogLineOutdated(logLine))
                {
                    return;
                }

                //parse
                var match = GoldRewardExtendedRegex.Match(logLine);
                var rewardInfo = new GoldRewardViewModel(logLine, match.Groups["amount"].Value, match.Groups["origin"].Value, match.Groups["origindata"].Value);

                _achievementProvider.ProvideComment(rewardInfo);

                _mainWindowModel?.TransactionList.AddTransaction(new TransactionViewModel { Difference = rewardInfo.Amount, Category = rewardInfo.Category, Comment = rewardInfo.Comment });
            }
        }

        private bool IsLogLineOutdated(string logLine)
        {
            DateTime loglinetime;
            if (logLine.Length > 20 && DateTime.TryParse(logLine.Substring(2, 16), out loglinetime))
            {
                if (loglinetime > DateTime.Now)
                {
                    loglinetime = loglinetime.AddDays(-1);
                }

                var latestTransaction = _mainWindowModel.TransactionList.Transactions.OrderByDescending(t => t.Moment).FirstOrDefault();
                if (latestTransaction != null)
                {
                    var latestTransactionTime = latestTransaction.Moment;

                    if (loglinetime <= latestTransactionTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void InitializeMainWindow()
        {
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow
                {
                    Width = Settings.CollectionWindowWidth,
                    Height = Settings.CollectionWindowHeight,
                    MaxHeight = SystemParameters.PrimaryScreenHeight,
                    MaxWidth = SystemParameters.PrimaryScreenWidth,
                    Title = "Treasury",
                    DataContext = _mainWindowModel
                };

                _mainWindow.Closed += (sender, args) =>
                {
                    Settings.CollectionWindowWidth = _mainWindow.Width;
                    Settings.CollectionWindowHeight = _mainWindow.Height;

                    TransactionHelper.SaveTransactions(_mainWindowModel.TransactionList.Transactions, TransactionsFile);

                    _mainWindow = null;
                };
            }
        }

        public void OnUnload()
        {
            if (_mainWindow != null)
            {
                if (_mainWindow.IsVisible)
                {
                    _mainWindow.Close();
                }
                _mainWindow = null;
            }

            if (Settings != null)
            {
                Settings.SaveSettings(SettingsFile);
            }
        }

        public void OnUpdate()
        {
            // no-op
        }
    }
}
