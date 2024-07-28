using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using System.Windows;

namespace RetailCommanderDesktop
{
    public partial class DeleteEmployeeForm : Window
    {
        public DeleteEmployeeForm(SqliteData dataAccess, ConfigurationForm configurationForm)
        {
            InitializeComponent();
            var viewModel = new DeleteEmployeeViewModel(dataAccess, configurationForm);
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
