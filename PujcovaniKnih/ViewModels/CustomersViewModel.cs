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
    /// ViewModel responsible for managing customers,
    /// providing CRUD operations and notifying the UI of changes.
    /// </summary>
    public class CustomersViewModel : INotifyPropertyChanged
    {
        // Collection of customers for data binding in the UI
        public ObservableCollection<Customer> Customers { get; set; } = new();
        private List<Customer> allCustomersCache = new();

        private Customer selectedCustomer;
        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                OnPropertyChanged();    // Notify the UI that the property has changed
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
                FilterCustomers();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand NewCommand { get; }

        public CustomersViewModel()
        {
            SelectedCustomer = new Customer();
            LoadCustomers();

            SaveCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(SelectedCustomer.Name) || string.IsNullOrWhiteSpace(SelectedCustomer.Phone))
                {
                    MessageBox.Show("Vyplňte prosím jméno a telefon.");
                    return;
                }

                if (SelectedCustomer.Id == 0)
                {
                    Database.AddCustomer(SelectedCustomer);
                }
                else
                {
                    Database.UpdateCustomer(SelectedCustomer);
                }

                LoadCustomers();
                SelectedCustomer = new Customer();
            });

            DeleteCommand = new RelayCommand(_ =>
            {
                if (MessageBox.Show("Opravdu smazat zákazníka?", "Potvrzení", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Database.DeleteCustomer(SelectedCustomer.Id);
                    LoadCustomers();
                    SelectedCustomer = new Customer();
                }
            }, _ => SelectedCustomer != null && SelectedCustomer.Id > 0);

            NewCommand = new RelayCommand(_ =>
            {
                SelectedCustomer = new Customer();
            });
        }

        private void FilterCustomers()
        {
            Customers.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var c in allCustomersCache) Customers.Add(c);
            }
            else
            {
                var filtered = allCustomersCache.Where(c =>
                    c.Name.ToLower().Contains(SearchText.ToLower()) ||
                    (c.Email != null && c.Email.ToLower().Contains(SearchText.ToLower())));

                foreach (var c in filtered) Customers.Add(c);
            }
        }

        public void LoadCustomers()
        {
            allCustomersCache = Database.GetAllCustomers();
            FilterCustomers();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
