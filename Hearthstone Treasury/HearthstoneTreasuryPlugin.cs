using Hearthstone_Treasury.Controls;
using Hearthstone_Treasury.ViewModels;
using System;
using System.IO;
using System.Windows;
using ReactiveUI;
using Hearthstone_Deck_Tracker;

namespace Hearthstone_Treasury
{
    public class HearthstoneTreasuryPlugin : Hearthstone_Deck_Tracker.Plugins.IPlugin
    {
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

            Settings.ResetDataFilesCommand.Subscribe(_ => {
                try
                {
                    if (File.Exists(AchievementsFile))
                    {
                        File.Delete(AchievementsFile);
                    }
                }
                catch (Exception) {
                    //no-op
                }

                _achievementProvider = CreateNewAchievementProvider();
            });

            _achievementProvider = CreateNewAchievementProvider();

            var transactions = TransactionHelper.LoadTransactions(TransactionsFile) ?? new ReactiveList<TransactionViewModel>() { ChangeTrackingEnabled = true };
            var transactionList = new TransactionListViewModel(transactions);

            var logHandler = new RachelleLogHandler(_achievementProvider, transactionList);

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

            Hearthstone_Deck_Tracker.API.LogEvents.OnRachelleLogLine.Add(logHandler.HandleRachelleLogUpdate);
        }

        private AchievementProvider CreateNewAchievementProvider()
        {
            var pathToHearthstoneAchievementsFile = string.IsNullOrEmpty(Config.Instance.HearthstoneDirectory) ? "" : AchievementDbfFilePath;
            return AchievementProvider.Create(Settings, AchievementsFile, pathToHearthstoneAchievementsFile);
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
