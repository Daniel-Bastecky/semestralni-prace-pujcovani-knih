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

        private Customer? selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                OnPropertyChanged();    // Notify the UI that the property has changed
            }
        }

        public RelayCommand AddCustomerCommand { get; }
        public RelayCommand UpdateCustomerCommand { get; }
        public RelayCommand DeleteCustomerCommand { get; }

        public CustomersViewModel()
        {
            LoadCustomers();

            AddCustomerCommand = new RelayCommand(_ => AddCustomer(SelectedCustomer));
            UpdateCustomerCommand = new RelayCommand(_ => UpdateCustomer(SelectedCustomer));
            DeleteCustomerCommand = new RelayCommand(_ =>
            {
                if (SelectedCustomer != null)
                    DeleteCustomer(SelectedCustomer.Id);
            });

            SelectedCustomer = new Customer();
        }

        public void LoadCustomers()
        {
            Customers.Clear();
            var customers = Database.GetAllCustomers();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }

        public void AddCustomer(Customer customer)
        {
            Database.AddCustomer(customer);
            LoadCustomers();
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                return;
            }

            Database.UpdateCustomer(customer);
            LoadCustomers();
        }

        public void DeleteCustomer(int customerId)
        {
            Database.DeleteCustomer(customerId);
            LoadCustomers();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
