using Hearthstone_Deck_Tracker;
using Hearthstone_Treasury.ViewModels;
using System.Collections.ObjectModel;
using System.IO;

namespace Hearthstone_Treasury
{
    public static class TransactionHelper
    {
        public static ObservableCollection<TransactionViewModel> LoadTransactions(string path)
        {
            if (File.Exists(path))
            {
                return XmlManager<ObservableCollection<TransactionViewModel>>.Load(path);
            }
            return null;
        }

        public static void SaveTransactions(ObservableCollection<TransactionViewModel> transactions, string path)
        {
            XmlManager<ObservableCollection<TransactionViewModel>>.Save(path, transactions);
        }
    }
}
