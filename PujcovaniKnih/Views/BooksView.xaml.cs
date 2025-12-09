using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PujcovaniKnih.ViewModels;

namespace PujcovaniKnih.Views
{
    public partial class BooksView : UserControl
    {
        public BooksView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BooksViewModel viewModel)
            {
                viewModel.LoadBooks();
            }
        }
    }
}
