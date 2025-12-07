using PujcovaniKnih.Commands;
using PujcovaniKnih.Data;
using PujcovaniKnih.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PujcovaniKnih.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing book loans,
    /// providing CRUD operations and notifying the UI of changes.
    /// </summary>
    public class LoansViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Loan> Loans { get; set; } = new();
        public ObservableCollection<Book> AllBooks { get; set; } = new();
        public ObservableCollection<Customer> AllCustomers { get; set; } = new();

        private List<Loan> allLoansCache = new();

        private Loan selectedLoan;
        public Loan SelectedLoan
        {
            get => selectedLoan;
            set
            {
                selectedLoan = value;
                OnPropertyChanged();
            }
        }

        private bool showActiveOnly = false;
        public bool ShowActiveOnly
        {
            get => showActiveOnly;
            set
            {
                showActiveOnly = value;
                OnPropertyChanged();
                FilterLoans();
            }
        }

        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                FilterLoans();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand NewCommand { get; }

        public LoansViewModel()
        {
            SelectedLoan = new Loan() { DateBorrowed = DateTime.Now };
            LoadData();

            SaveCommand = new RelayCommand(_ =>
            {
                if (SelectedLoan.CustomerId == 0 || SelectedLoan.BookId == 0)
                {
                    MessageBox.Show("Vyberte prosím zákazníka a knihu.");
                    return;
                }

                if (SelectedLoan.Id == 0)
                {
                    // Nová
                    var book = AllBooks.FirstOrDefault(b => b.Id == SelectedLoan.BookId);
                    if (book != null && !book.IsAvailable)
                    {
                        MessageBox.Show($"Kniha '{book.Title}' je již půjčená!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Database.AddLoan(SelectedLoan);
                    Database.SetBookAvailability(SelectedLoan.BookId, false);
                }
                else
                {
                    Database.UpdateLoan(SelectedLoan);
                    if (SelectedLoan.DateReturned != null)
                    {
                        Database.SetBookAvailability(SelectedLoan.BookId, true);
                    }
                }
                LoadData();
                SelectedLoan = new Loan() { DateBorrowed = DateTime.Now };
            });

            DeleteCommand = new RelayCommand(_ =>
            {
                if (MessageBox.Show("Smazat výpůjčku? Kniha bude uvolněna.", "Potvrzení", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Database.SetBookAvailability(SelectedLoan.BookId, true);
                    Database.DeleteLoan(SelectedLoan.Id);
                    LoadData();
                    SelectedLoan = new Loan() { DateBorrowed = DateTime.Now };
                }
            }, _ => SelectedLoan != null && SelectedLoan.Id > 0);

            NewCommand = new RelayCommand(_ => SelectedLoan = new Loan() { DateBorrowed = DateTime.Now });
        }

        private void FilterLoans()
        {
            Loans.Clear();

            IEnumerable<Loan> filtered = allLoansCache;

            if (ShowActiveOnly)
            {
                filtered = filtered.Where(l => l.DateReturned == null);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string term = SearchText.ToLower();
                filtered = filtered.Where(l =>
                    l.CustomerName.ToLower().Contains(term) ||
                    l.BookTitle.ToLower().Contains(term));
            }

            foreach (var loan in filtered) Loans.Add(loan);
        }

        public void LoadData()
        {
            AllCustomers.Clear();
            foreach (var c in Database.GetAllCustomers()) AllCustomers.Add(c);

            AllBooks.Clear();
            foreach (var b in Database.GetAllBooks()) AllBooks.Add(b);

            allLoansCache = Database.GetAllLoans();

            foreach (var loan in allLoansCache)
            {
                var customer = AllCustomers.FirstOrDefault(c => c.Id == loan.CustomerId);
                var book = AllBooks.FirstOrDefault(b => b.Id == loan.BookId);

                loan.CustomerName = customer?.Name ?? "neznámý";
                loan.BookTitle = book?.Title ?? "neznámá";
            }

            FilterLoans();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
