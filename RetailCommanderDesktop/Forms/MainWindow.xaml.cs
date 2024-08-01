using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using Microsoft.Extensions.Configuration;
using System.Windows;
using RetailCommanderLibrary.Database;

namespace RetailCommanderDesktop.Forms
{
    public partial class MainWindow : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly IConfiguration _config;
        private readonly ITranslationManager _translationManager;

        public MainWindow(IConfiguration config, ITranslationManager translationManager)
        {
            InitializeComponent();
            _config = config;
            _translationManager = translationManager;
            _dataAccess = new SqliteData(new SqliteDataAccess(_config));
            var viewModel = new MainWindowViewModel(_dataAccess, _config, _translationManager);
            DataContext = viewModel;
        }
    }
}
