using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using System.Windows;

namespace RetailCommanderDesktop.Forms
{
    public partial class DeleteEmployeeForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationFormViewModel _configurationFormViewModel;

        public DeleteEmployeeForm(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _configurationFormViewModel = configurationFormViewModel;
            var viewModel = new DeleteEmployeeViewModel(_dataAccess, _configurationFormViewModel);
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
