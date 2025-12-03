using PujcovaniKnih.Commands;
using PujcovaniKnih.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PujcovaniKnih.ViewModels
{
    /// <summary>
    /// Main ViewModel that holds all other ViewModels and controls which one is currently displayed.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public BooksViewModel BooksVM { get;} = new();
        public CustomersViewModel CustomersVM { get;} = new();
        public LoansViewModel LoansVM { get;} = new();

        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        // Commands to switch views
        public RelayCommand ShowBooksCommand { get; }
        public RelayCommand ShowCustomersCommand { get; }
        public RelayCommand ShowLoansCommand { get; }

        public MainViewModel()
        {
            CurrentView = BooksVM;
            ShowBooksCommand = new RelayCommand(_ => CurrentView = BooksVM);
            ShowCustomersCommand = new RelayCommand(_ => CurrentView = CustomersVM);
            ShowLoansCommand = new RelayCommand(_ => CurrentView = LoansVM);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
