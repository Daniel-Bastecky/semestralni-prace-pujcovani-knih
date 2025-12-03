using PujcovaniKnih.Data;
using SQLitePCL;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PujcovaniKnih
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Batteries_V2.Init();
            Database.Initialize();
            InitializeComponent();
        }
    }

}
