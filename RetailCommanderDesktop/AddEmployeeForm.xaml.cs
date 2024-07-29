using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using System.Windows;

namespace RetailCommanderDesktop
{
    public partial class AddEmployeeForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationFormViewModel _configurationFormViewModel;

        public AddEmployeeForm(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _configurationFormViewModel = configurationFormViewModel;
            var viewModel = new AddEmployeeFormViewModel(_dataAccess, _configurationFormViewModel);
            viewModel.ShowMessage += ShowMessage;
            viewModel.CloseWindow += CloseWindow;
            DataContext = viewModel;
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void CloseWindow()
        {
            this.Close();
        }
    }
}
