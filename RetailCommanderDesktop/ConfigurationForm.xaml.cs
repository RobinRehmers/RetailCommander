using RetailCommanderLibrary.Data;
using RetailCommanderDesktop.ViewModels;
using System.Windows;

namespace RetailCommanderDesktop
{
    public partial class ConfigurationForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = _mainWindowViewModel.ConfigurationFormViewModel;
        }
    }
}
