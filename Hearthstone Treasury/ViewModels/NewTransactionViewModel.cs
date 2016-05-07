using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hearthstone_Treasury.ViewModels
{
    public class NewTransactionViewModel : ReactiveObject
    {
        public NewTransactionViewModel()
        {
            CreateTransaction = ReactiveCommand.Create();
        }

        public ReactiveCommand<object> CreateTransaction { get; private set; }

        [Reactive]
        public int? Difference { get; set; }

        [Reactive]
        public CategoryEnum? Category { get; set; }

        [Reactive]
        public string Comment { get; set; }

        internal void Reset()
        {
            Difference = null;
            Category = null;
            Comment = "";
        }
    }
}
