using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using Microsoft.Extensions.Configuration;
using System.Windows;
using RetailCommanderLibrary.Database;

namespace RetailCommanderDesktop
{
    public partial class MainWindow : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly IConfiguration _config;
        private readonly ITranslationManager _translationManager;

        public MainWindow(IConfiguration config)
        {
            InitializeComponent();
            _config = config;
            _dataAccess = new SqliteData(new SqliteDataAccess(_config));
            _translationManager = new TranslationManager(_dataAccess);
            //AddInitialTranslations();
            var viewModel = new MainWindowViewModel(_dataAccess, _config, _translationManager);
            DataContext = viewModel;
        }

        private void AddInitialTranslations()
        {
            //_translationManager.SaveTranslation("LanguageLabel", "DE", "Sprache");
        }
    }
}
