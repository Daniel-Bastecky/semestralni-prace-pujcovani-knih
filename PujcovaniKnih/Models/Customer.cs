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
    /// Represents a customer in the library.
    /// </summary>
    public class Customer : INotifyPropertyChanged
    {
        private int id;
        private string name = string.Empty;
        private string? email;
        private string phone = string.Empty;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        public string? Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(); }
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
