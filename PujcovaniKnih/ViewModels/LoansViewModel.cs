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
        // Collection of loans for data binding in the UI
        public ObservableCollection<Loan> Loans { get; set; } = new();

        public ObservableCollection<Book> AllBooks { get; set; } = new();
        public ObservableCollection<Customer> AllCustomers { get; set; } = new();

        private Loan? selectedLoan;
        public Loan? SelectedLoan
        {
            get => selectedLoan;
            set
            {
                selectedLoan = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddLoanCommand { get; }
        public RelayCommand UpdateLoanCommand { get; }
        public RelayCommand DeleteLoanCommand { get; }

        public LoansViewModel()
        {
            LoadData();

            AddLoanCommand = new RelayCommand(_ =>
            {
                if (SelectedLoan == null) return;

                // najde vybranou knihu v seznamu
                var bookToBorrow = AllBooks.FirstOrDefault(b => b.Id == SelectedLoan.BookId);

                if (bookToBorrow != null)
                {
                    if (!bookToBorrow.IsAvailable)
                    {
                        MessageBox.Show("Tato kniha je již půjčená!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    AddLoan(SelectedLoan);

                    Database.SetBookAvailability(bookToBorrow.Id, false);

                    LoadData();
                    SelectedLoan = new Loan() { DateBorrowed = DateTime.Now }; // reset formuláře
                }
                else
                {
                    MessageBox.Show("Vyberte prosím knihu.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            UpdateLoanCommand = new RelayCommand(_ =>
            {
                if (SelectedLoan != null)
                {
                    UpdateLoan(SelectedLoan);

                    if (SelectedLoan.DateReturned != null)
                    {
                        Database.SetBookAvailability(SelectedLoan.BookId, true);
                        LoadData();
                    }
                }
            });

            DeleteLoanCommand = new RelayCommand(_ =>
            {
                if (SelectedLoan != null)
                {
                    Database.SetBookAvailability(SelectedLoan.BookId, true);

                    DeleteLoan(SelectedLoan.Id);
                    LoadData();
                    SelectedLoan = new Loan() { DateBorrowed = DateTime.Now };
                }
            });

            SelectedLoan = new Loan() { DateBorrowed = DateTime.Now };
        }

        public void LoadData()
        {
            // načtení výpůjček
            Loans.Clear();
            var loans = Database.GetAllLoans();
            foreach (var loan in loans)
            {
                Loans.Add(loan);
            }

            //načtení knih
            AllBooks.Clear();
            var books = Database.GetAllBooks();
            foreach (var book in books) 
            {
                AllBooks.Add(book);
            }

            // načtení zákazníků
            AllCustomers.Clear();
            var customers = Database.GetAllCustomers();
            foreach (var customer in customers)
            {
                AllCustomers.Add(customer);
            }
        }

        public void AddLoan(Loan loan) => Database.AddLoan(loan);
        public void UpdateLoan(Loan loan) => Database.UpdateLoan(loan);
        public void DeleteLoan(int loanId) => Database.DeleteLoan(loanId);


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
