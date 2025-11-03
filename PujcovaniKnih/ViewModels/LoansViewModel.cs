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

        private Loan? selectedLoan;
        public Loan? SelectedLoan
        {
            get => selectedLoan;
            set
            {
                selectedLoan = value;
                OnPropertyChanged();    // Notify the UI that the property has changed
            }
        }

        public LoansViewModel()
        {
            LoadLoans();
        }

        public void LoadLoans()
        {
            Loans.Clear();
            var loans = Database.GetAllLoans();
            foreach (var loan in loans)
            {
                Loans.Add(loan);
            }
        }

        public void AddLoan(Loan loan)
        {
            Database.AddLoan(loan);
            LoadLoans();
        }

        public void UpdateLoan(Loan loan)
        {
            if (loan == null)
            {
                return;
            }

            Database.UpdateLoan(loan);
            LoadLoans();
        }

        public void DeleteLoan(int loanId)
        {
            Database.DeleteLoan(loanId);
            LoadLoans();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
