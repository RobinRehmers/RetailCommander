using RetailCommanderLibrary.Data;
using RetailCommanderDesktop.Commands;
using System.Windows.Input;

namespace RetailCommanderDesktop.ViewModels
{
    public class ConfigurationFormViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ICommand AddEmployeeCommand { get; }
        public ICommand RemoveEmployeeCommand { get; }

        public ConfigurationFormViewModel(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel)
        {
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeForm);
            RemoveEmployeeCommand = new RelayCommand(OpenRemoveEmployeeForm);
        }

        private void OpenAddEmployeeForm(object parameter)
        {
            var addEmployeeForm = new AddEmployeeForm(_dataAccess, this);
            addEmployeeForm.Show();
        }

        private void OpenRemoveEmployeeForm(object parameter)
        {
            var deleteEmployeeForm = new DeleteEmployeeForm(_dataAccess, this);
            deleteEmployeeForm.ShowDialog();
        }

        public void LoadEmployeeData()
        {
            _mainWindowViewModel.LoadEmployees(null);
        }
    }
}
