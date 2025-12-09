using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using PujcovaniKnih.ViewModels;

namespace PujcovaniKnih.Views
{
    public partial class LoansView : UserControl
    {
        public LoansView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoansViewModel viewModel)
            {
                viewModel.LoadData();
            }
        }
    }
}
