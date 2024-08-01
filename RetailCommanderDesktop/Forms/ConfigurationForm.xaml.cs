using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using System.Windows;
using System.Windows.Controls;

namespace RetailCommanderDesktop.Forms
{
    public partial class ConfigurationForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel, ITranslationManager translationManager)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = new ConfigurationFormViewModel(_dataAccess, _mainWindowViewModel, translationManager);
        }
    }
}
