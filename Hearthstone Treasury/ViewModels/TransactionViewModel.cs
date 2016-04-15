using System;
using System.Windows;

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
    public class TransactionViewModel : DependencyObject
    {
        public TransactionViewModel()
        {
            Moment = DateTime.UtcNow;
        }

        public static readonly DependencyProperty MomentProperty = DependencyProperty.Register("Moment", typeof(DateTime), typeof(TransactionViewModel), new UIPropertyMetadata());
        public DateTime Moment
        {
            get
            {
                return (DateTime)GetValue(MomentProperty);
            }
            set
            {
                SetValue(MomentProperty, value);
            }
        }

        public static readonly DependencyProperty DifferenceProperty = DependencyProperty.Register("Difference", typeof(int), typeof(TransactionViewModel), new UIPropertyMetadata(0));
        public int Difference
        {
            get
            {
                return (int)GetValue(DifferenceProperty);
            }
            set
            {
                SetValue(DifferenceProperty, value);
            }
        }

        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(CategoryEnum), typeof(TransactionViewModel), new UIPropertyMetadata());
        public CategoryEnum Category
        {
            get
            {
                return (CategoryEnum)GetValue(CategoryProperty);
            }
            set
            {
                SetValue(CategoryProperty, value);
            }
        }
    }
}
