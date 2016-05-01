using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hearthstone_Treasury.ViewModels
{
    [Serializable]
    public enum CategoryEnum
    {
        Pack,
        Arena,
        AdventureWing,
        Daily,
        Wins
    }

    [Serializable]
    public class TransactionViewModel : ReactiveObject
    {
        public TransactionViewModel()
        {
            Id = Guid.NewGuid();
            Moment = DateTime.Now;
        }

        [Reactive]
        public Guid Id { get; set; }

        [Reactive]
        public DateTime Moment { get; set; }

        [Reactive]
        public int Difference { get; set; }

        [Reactive]
        public CategoryEnum Category { get; set; }

        [Reactive]
        public string Comment { get; set; }
    }
}
