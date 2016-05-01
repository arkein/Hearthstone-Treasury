using Hearthstone_Deck_Tracker;
using Hearthstone_Treasury.ViewModels;
using System.IO;
using ReactiveUI;

namespace Hearthstone_Treasury
{
    public static class TransactionHelper
    {
        public static ReactiveList<TransactionViewModel> LoadTransactions(string path)
        {
            if (File.Exists(path))
            {
                var list = XmlManager<ReactiveList<TransactionViewModel>>.Load(path);
                list.ChangeTrackingEnabled = true;
                return list;
            }
            return null;
        }

        public static void SaveTransactions(ReactiveList<TransactionViewModel> transactions, string path)
        {
            XmlManager<ReactiveList<TransactionViewModel>>.Save(path, transactions);
        }
    }
}
