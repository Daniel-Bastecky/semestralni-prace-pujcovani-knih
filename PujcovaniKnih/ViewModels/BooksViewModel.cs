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
    /// Manages the logic for the Books section, including loading, filtering, and CRUD operations.
    /// </summary>
    public class BooksViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> Books { get; set; } = new();

        private List<Book> allBooksCache = new();

        private Book selectedBook;
        public Book SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                OnPropertyChanged();
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
                FilterBooks();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand NewCommand { get; }

        public BooksViewModel()
        {
            SelectedBook = new Book();
            LoadBooks();

            SaveCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(SelectedBook.Title) || string.IsNullOrWhiteSpace(SelectedBook.Author))
                {
                    MessageBox.Show("Vyplňte prosím název a autora.");
                    return;
                }

                if (SelectedBook.Id == 0)
                {
                    Database.AddBook(SelectedBook);
                }
                else
                {
                    Database.UpdateBook(SelectedBook);
                }

                LoadBooks();
                SelectedBook = new Book();
            });

            DeleteCommand = new RelayCommand(_ =>
            {
                bool isUsed = Database.GetAllLoans().Any(l => l.BookId == SelectedBook.Id);

                if (isUsed)
                {
                    MessageBox.Show($"Knihu '{SelectedBook.Title}' nelze smazat, protože je součástí existující výpůjčky.\n\n",
                                    "Nelze smazat", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show("Opravdu smazat?", "Potvrzení", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Database.DeleteBook(SelectedBook.Id);
                    LoadBooks();
                    SelectedBook = new Book();
                }
            }, _ => SelectedBook != null && SelectedBook.Id > 0);

            NewCommand = new RelayCommand(_ =>
            {
                SelectedBook = new Book();
            });
        }

        private void FilterBooks()
        {
            Books.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var b in allBooksCache) Books.Add(b);
            }
            else
            {
                var filtered = allBooksCache.Where(b =>
                    b.Title.ToLower().Contains(SearchText.ToLower()) ||
                    b.Author.ToLower().Contains(SearchText.ToLower()));

                foreach (var b in filtered) Books.Add(b);
            }
        }

        public void LoadBooks()
        {
            allBooksCache = Database.GetAllBooks();
            FilterBooks();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
