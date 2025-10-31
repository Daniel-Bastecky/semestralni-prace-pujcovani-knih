using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PujcovaniKnih.Data
{
    /// <summary>
    /// Class responsible for initializing the SQLite database and creating the required tables.
    /// </summary>
    public static class Database
    {
        private static string connectionString = "Data Source=library.db;Version=3;";

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
    }
}
