using Hearthstone_Treasury.Controls;
using Hearthstone_Treasury.ViewModels;
using System;
using System.IO;
using System.Windows;
using ReactiveUI;
using Hearthstone_Treasury.Enums;
using System.Linq;

namespace Hearthstone_Treasury
{
    public class HearthstoneTreasuryPlugin : Hearthstone_Deck_Tracker.Plugins.IPlugin
    {
        internal static string PluginDataDir => Path.Combine(Hearthstone_Deck_Tracker.Config.Instance.DataDir, "Treasury");
        internal static string TransactionsFile => Path.Combine(PluginDataDir, "transactions.xml");
        internal static string SettingsFile => Path.Combine(PluginDataDir, "treasury.config.xml");

        private PluginSettingsViewModel Settings { get; set; }

        private readonly TreasuryMenuItem _menuItem = new TreasuryMenuItem { Header = "Treasury" };

        public System.Windows.Controls.MenuItem MenuItem => _menuItem;

        private MainWindowViewModel _mainWindowModel;

        private MainWindow _mainWindow = null;
        private SettingsWindow _settingsWindow = null;

        public string Author => "Arkein";

        public string ButtonText => "Settings";

        public string Description => "Gold tracking plugin for Hearthstone Deck Tracker.";

        public string Name => "Treasury";

        public Version Version => new Version(0, 0, 0, 1);

        public void OnButtonPress()
        {
            if (_settingsWindow == null)
            {
                InitializeSettingsWindow();
                _settingsWindow.Show();
            }
            else
            {
                _settingsWindow.Activate();
            }
        }

        public void OnLoad()
        {
            if (!Directory.Exists(PluginDataDir))
            {
                Directory.CreateDirectory(PluginDataDir);
            }

            Settings = PluginSettingsViewModel.LoadSettings(SettingsFile);

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

        public void InitializeSettingsWindow()
        {
            if (_settingsWindow == null)
            {
                _settingsWindow = new SettingsWindow()
                {
                    Width = Settings.CollectionWindowWidth,
                    Height = Settings.CollectionWindowHeight,
                    MaxHeight = SystemParameters.PrimaryScreenHeight,
                    MaxWidth = SystemParameters.PrimaryScreenWidth,
                    Title = "Treasury Settings",
                    DataContext = Settings
                };

                _settingsWindow.Closed += (sender, args) =>
                {
                    Settings.SaveSettings(SettingsFile);
                    _settingsWindow = null;
                };
            }
        }

        public void OnUnload()
        {
            if (_settingsWindow != null)
            {
                if (_settingsWindow.IsVisible)
                {
                    _settingsWindow.Close();
                }
                _settingsWindow = null;
            }

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
