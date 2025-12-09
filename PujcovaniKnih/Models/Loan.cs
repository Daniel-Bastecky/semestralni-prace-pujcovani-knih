using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PujcovaniKnih.Models
{
    /// <summary>
    /// Represents a loan in the library.
    /// </summary>
    public class Loan : INotifyPropertyChanged
    {
        private int id;
        private int customerId;
        private int bookId;
        private DateTime dateBorrowed;
        private DateTime? dateReturned;

        private string customerName = string.Empty;
        private string bookTitle = string.Empty;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        public int CustomerId
        {
            get => customerId;
            set { customerId = value; OnPropertyChanged(); }
        }

        public int BookId
        {
            get => bookId;
            set { bookId = value; OnPropertyChanged(); }
        }

        public DateTime DateBorrowed
        {
            get => dateBorrowed;
            set { dateBorrowed = value; OnPropertyChanged(); }
        }

        public DateTime? DateReturned
        {
            get => dateReturned;
            set { dateReturned = value; OnPropertyChanged(); }
        }

        public string CustomerName
        {
            get => customerName;
            set { customerName = value; OnPropertyChanged(); }
        }

        public string BookTitle
        {
            get => bookTitle;
            set { bookTitle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Event that notifies the UI when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
