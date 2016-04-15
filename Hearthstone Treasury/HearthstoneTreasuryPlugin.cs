using Hearthstone_Treasury.Controls;
using Hearthstone_Treasury.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;

namespace Hearthstone_Treasury
{
    //1. Truly observable collection
    //2. UTC to Local time
    //3. Listen to 3 wins event
    //4. Migrate to Reactive
    public class HearthstoneTreasuryPlugin : Hearthstone_Deck_Tracker.Plugins.IPlugin
    {
        internal static string PluginDataDir => Path.Combine(Hearthstone_Deck_Tracker.Config.Instance.DataDir, "Treasury");
        internal static string TransactionsFile => Path.Combine(PluginDataDir, "transactions.xml");

        public static PluginSettings Settings { get; set; }

        private readonly TreasuryMenuItem _menuItem = new TreasuryMenuItem { Header = "Treasury" };

        public System.Windows.Controls.MenuItem MenuItem => _menuItem;

        private MainWindowViewModel _mainWindowModel;

        private MainWindow _mainWindow = null;

        public string Author => "Arkein";

        public string ButtonText => "Settings";

        public string Description => "Gold tracking plugin for Hearthstone Deck Tracker.";

        public string Name => "Treasury";

        public Version Version => new Version(0, 0, 0, 1);

        public void OnButtonPress()
        {
            throw new NotImplementedException();
        }

        public void OnLoad()
        {
            if (!Directory.Exists(PluginDataDir))
            {
                Directory.CreateDirectory(PluginDataDir);
            }

            Settings = PluginSettings.LoadSettings(PluginDataDir);

            var transactions = TransactionHelper.LoadTransactions(TransactionsFile) ?? new ObservableCollection<TransactionViewModel>()
            {
                new TransactionViewModel() { Moment = DateTime.UtcNow, Difference=-100, Category = CategoryEnum.Pack },
                new TransactionViewModel() { Moment = DateTime.UtcNow.AddHours(1), Difference=+10, Category = CategoryEnum.Arena }
            };

            _mainWindowModel = new MainWindowViewModel(transactions);

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

                    TransactionHelper.SaveTransactions(_mainWindowModel.Transactions, TransactionsFile);

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
                Settings.SaveSettings(PluginDataDir);
            }
        }

        public void OnUpdate()
        {
            // no-op
        }
    }
}
