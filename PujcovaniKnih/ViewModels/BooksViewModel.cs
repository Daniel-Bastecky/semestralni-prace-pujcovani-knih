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
    /// ViewModel responsible for managing the library's book collection,
    /// providing operations to load, add, update, and delete books,
    /// and notifying the UI about any data changes for proper binding.
    /// </summary>
    public class BooksViewModel : INotifyPropertyChanged
    {
        // Collection of books for data binding in the UI
        public ObservableCollection<Book> Books { get; set; } = new();

        private Book? selectedBook;
        public Book? SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                OnPropertyChanged();    // Notify the UI that the property has changed
            }
        }

        // commands for buttons
        public RelayCommand AddBookCommand { get; }
        public RelayCommand UpdateBookCommand { get; }
        public RelayCommand DeleteBookCommand { get; }

        public BooksViewModel()
        {
            LoadBooks();

            AddBookCommand = new RelayCommand(_ =>
            {
                if (SelectedBook != null)
                {
                    AddBook(SelectedBook);
                    SelectedBook = new Book(); // reset
                }
            });

            UpdateBookCommand = new RelayCommand(_ =>
            {
                if (SelectedBook != null)
                {
                    UpdateBook(SelectedBook);
                }
            });

            DeleteBookCommand = new RelayCommand(_ =>
            {
                if (SelectedBook != null)
                {
                    DeleteBook(SelectedBook.Id);
                    SelectedBook = new Book(); // reset
                }
            });

            SelectedBook = new Book();
        }

        public void LoadBooks()
        {
            Books.Clear();
            var books = Database.GetAllBooks();
            foreach(var book in books)
            {
                Books.Add(book);
            }
        }

        public void AddBook(Book book)
        {
            Database.AddBook(book);
            LoadBooks();
        }

        public void UpdateBook(Book book)
        {
            if(book == null)
            {
                return;
            }

            Database.UpdateBook(book);
            LoadBooks();
        }

        public void DeleteBook(int bookId)
        {
            Database.DeleteBook(bookId);
            LoadBooks();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
