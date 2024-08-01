using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System;
using System.Windows.Input;
using RetailCommanderDesktop.Forms;

namespace RetailCommanderDesktop.ViewModels
{
    public class AddEmployeeFormViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationFormViewModel _configurationFormViewModel;

        public event Action<string> ShowMessage;
        public event Action CloseWindow;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Hours { get; set; }
        public ICommand AddEmployeeCommand { get; }

        public AddEmployeeFormViewModel(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel)
        {
            _dataAccess = dataAccess;
            _configurationFormViewModel = configurationFormViewModel;
            AddEmployeeCommand = new RelayCommand(AddEmployee);
        }

        private void AddEmployee(object parameter)
        {
            try
            {
                _dataAccess.AddEmployee(FirstName, LastName, Hours, 0);
                ShowMessage?.Invoke("Employee added successfully!");
                _configurationFormViewModel.LoadEmployeeData();
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                ShowMessage?.Invoke($"Error adding employee: {ex.Message}");
            }
        }
    }
}
