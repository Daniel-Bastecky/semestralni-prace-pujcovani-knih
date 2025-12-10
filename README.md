# Library Loan System (Simple Database Application)

* **Entities:** Books, Customers, Loans
* Each book is tracked as borrowed or available. Each loan is assigned to a customer and a book.
* The application has 3 tabs, one for each entity, and each contains a list of entity records that can be created, updated, and deleted.
* It is not possible to delete a book or a customer if they are listed in any loan record.
* It is not possible to insert a record with empty parameters, such as a name.
* It is not possible to enter a return date earlier than the borrow date.
* The lists in individual tabs can be filtered: for books by name and author, for customers by name and email, and for loans by name and book. It is also possible to filter out loans where the book has not yet been returned.
* **Data storage using SQLite (Embedded Database)**
* **Architecture:** MVVM + WPF
