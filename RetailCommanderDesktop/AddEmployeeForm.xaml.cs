using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using System.Windows;

namespace RetailCommanderDesktop
{
    public partial class AddEmployeeForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationForm _configurationForm;

        public AddEmployeeForm(SqliteData dataAccess, ConfigurationForm configurationForm)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _configurationForm = configurationForm;
            var viewModel = new AddEmployeeFormViewModel(_dataAccess, _configurationForm);
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