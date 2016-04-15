using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Hearthstone_Treasury.ViewModels
{
    public class MainWindowViewModel : DependencyObject
    {
        public MainWindowViewModel(ObservableCollection<TransactionViewModel> transactionsList)
        {
            Balance = HearthstoneTreasuryPlugin.Settings.InitialBalance;

            Transactions = transactionsList;
            Transactions.CollectionChanged += UpdateBalance;
        }

        private void UpdateBalance(object sender, NotifyCollectionChangedEventArgs e)
        {
            Balance = HearthstoneTreasuryPlugin.Settings.InitialBalance + Transactions.Sum(x => x.Difference);
        }

        public static readonly DependencyProperty TransactionsProperty = DependencyProperty.Register("Transactions", typeof(ObservableCollection<TransactionViewModel>), typeof(MainWindowViewModel), new UIPropertyMetadata(new ObservableCollection<TransactionViewModel>()));
        public ObservableCollection<TransactionViewModel> Transactions
        {
            get
            {
                return (ObservableCollection<TransactionViewModel>)GetValue(TransactionsProperty);
            }
            set
            {
                SetValue(TransactionsProperty, value);
            }
        }

        public static readonly DependencyProperty BalanceProperty = DependencyProperty.Register("Balance", typeof(int), typeof(MainWindowViewModel));
        public int Balance
        {
            get
            {
                return (int)GetValue(BalanceProperty);
            }
            set
            {
                SetValue(BalanceProperty, value);
            }
        }
    }
}
