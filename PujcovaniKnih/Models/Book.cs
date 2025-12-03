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
    /// Represents a book in the library.
    /// </summary>
    public class Book : INotifyPropertyChanged
    {
        private int id;
        private string title = string.Empty;
        private string author = string.Empty;
        private bool isAvailable;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => title;
            set { title = value; OnPropertyChanged(); }
        }

        public string Author
        {
            get => author;
            set { author = value; OnPropertyChanged(); }
        }

        public bool IsAvailable
        {
            get => isAvailable;
            set { isAvailable = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
