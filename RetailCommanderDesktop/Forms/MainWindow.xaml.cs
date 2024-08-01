using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using Microsoft.Extensions.Configuration;
using System.Windows;
using RetailCommanderLibrary.Database;

namespace RetailCommanderDesktop.Forms
{
    public partial class MainWindow : Window
    {
        public MainWindow(IConfiguration config)
        {
            InitializeComponent();
            var dataAccess = new SqliteData(new SqliteDataAccess(config));
            var translationManager = new TranslationManager(dataAccess);
            DataContext = new MainWindowViewModel(dataAccess, config, translationManager);
        }
    }
}
