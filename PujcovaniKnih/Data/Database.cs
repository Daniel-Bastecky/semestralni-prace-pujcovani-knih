using Microsoft.Data.Sqlite;
using PujcovaniKnih.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PujcovaniKnih.Data
{
    /// <summary>
    /// Class responsible for initializing the SQLite database and creating the required tables.
    /// </summary>
    public static class Database
    {
        private static readonly string dbPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "library.db");
        private static readonly string connectionString = $"Data Source={dbPath};";

        /// <summary>
        /// Creates the database and creates the tables Books, Customers and Loans.
        /// </summary>
        public static void Initialize()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string createBooksTable = @"CREATE TABLE IF NOT EXISTS Books(
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Title TEXT NOT NULL,
                                    Author TEXT NOT NULL,
                                    IsAvailable INTEGER NOT NULL
                                    );";
            using var createBooksCmd = new SqliteCommand(createBooksTable, connection);
            createBooksCmd.ExecuteNonQuery();

            string createCustomersTable = @"CREATE TABLE IF NOT EXISTS Customers(
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL,
                                    Email TEXT,
                                    Phone TEXT NOT NULL
                                    );";
            using var createCustomersCmd = new SqliteCommand(createCustomersTable, connection);
            createCustomersCmd.ExecuteNonQuery();

            string createLoansTable = @"CREATE TABLE IF NOT EXISTS Loans(
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    CustomerId INTEGER NOT NULL,
                                    BookId INTEGER NOT NULL,
                                    DateBorrowed TEXT NOT NULL,
                                    DateReturned TEXT,
                                    FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
                                    FOREIGN KEY(BookId) REFERENCES Books(Id)
                                    );";
            using var createLoansCmd = new SqliteCommand(createLoansTable, connection);
            createLoansCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all book records from the database and maps them to a list of Book objects.
        /// </summary>
        public static List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "SELECT Id, Title, Author, IsAvailable FROM Books;";
            using var cmd = new SqliteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new Book
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    IsAvailable = reader.GetBoolean(3) == true
                });
            }

            return books;
        }

        public static void AddBook(Book book)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO Books (Title, Author, IsAvailable) VALUES (@Title,@Author,@IsAvailable);";
            using var cmd = new SqliteCommand(query,connection);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@IsAvailable", book.IsAvailable ? 1 : 0);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateBook(Book book)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "UPDATE Books SET Title=@Title, Author=@Author, IsAvailable=@IsAvailable WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@IsAvailable", book.IsAvailable ? 1 : 0);
            cmd.Parameters.AddWithValue("@Id", book.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteBook(int bookId)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "DELETE FROM Books WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query,connection);
            cmd.Parameters.AddWithValue("@Id", bookId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all customer records from the database and maps them to a list of Customer objects.
        /// </summary>
        public static List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "SELECT Id, Name, Email, Phone FROM Customers;";
            using var cmd = new SqliteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Phone = reader.GetString(3)
                });
            }

            return customers;
        }

        public static void AddCustomer(Customer customer)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO Customers (Name, Email, Phone) VALUES (@Name,@Email,@Phone);";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateCustomer(Customer customer)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "UPDATE Customers SET Name=@Name, Email=@Email, Phone=@Phone WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.Parameters.AddWithValue("@Id", customer.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteCustomer(int customerId)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "DELETE FROM Customers WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", customerId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all loan records from the database and maps them to a list of Loan objects.
        /// </summary>
        public static List<Loan> GetAllLoans()
        {
            var loans = new List<Loan>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "SELECT Id, CustomerId, BookId, DateBorrowed, DateReturned FROM Loans;";
            using var cmd = new SqliteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                loans.Add(new Loan
                {
                    Id = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    BookId = reader.GetInt32(2),
                    DateBorrowed = DateTime.Parse(reader.GetString(3)),
                    DateReturned = reader.IsDBNull(4) ? (DateTime?)null : DateTime.Parse(reader.GetString(4))
                });
            }

            return loans;
        }

        public static void AddLoan(Loan loan)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = @"INSERT INTO Loans (CustomerId, BookId, DateBorrowed, DateReturned) VALUES (@CustomerId,@BookId,@DateBorrowed, @DateReturned);";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
            cmd.Parameters.AddWithValue("@BookId", loan.BookId);
            cmd.Parameters.AddWithValue("@DateBorrowed", loan.DateBorrowed.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@DateReturned", loan.DateReturned.HasValue ? loan.DateReturned.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateLoan(Loan loan)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = @"UPDATE Loans SET CustomerId=@CustomerId, BookId=@BookId, DateBorrowed=@DateBorrowed, DateReturned=@DateReturned WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
            cmd.Parameters.AddWithValue("@BookId", loan.BookId);
            cmd.Parameters.AddWithValue("@DateBorrowed", loan.DateBorrowed.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@DateReturned", loan.DateReturned.HasValue ? loan.DateReturned.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", loan.Id);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteLoan(int loanId)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "DELETE FROM Loans WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", loanId);
            cmd.ExecuteNonQuery();
        }

        public static void SetBookAvailability(int bookId, bool isAvailable)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "UPDATE Books SET IsAvailable=@IsAvailable WHERE Id=@Id;";
            using var cmd = new SqliteCommand(query, connection);

            cmd.Parameters.AddWithValue("@IsAvailable", isAvailable ? 1 : 0);
            cmd.Parameters.AddWithValue("@Id", bookId);

            cmd.ExecuteNonQuery();
        }
    }
}
