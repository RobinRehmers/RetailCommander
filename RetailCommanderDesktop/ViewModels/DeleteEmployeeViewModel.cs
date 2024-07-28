using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailCommanderDesktop.ViewModels
{
    public class DeleteEmployeeViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationForm _configurationForm;

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ICommand DeleteSelectedEmployeesCommand { get; }

        public event Action<string> ShowMessage;
        public event Action CloseWindow;

        public DeleteEmployeeViewModel(SqliteData dataAccess, ConfigurationForm configurationForm)
        {
            _dataAccess = dataAccess;
            _configurationForm = configurationForm;
            Employees = new ObservableCollection<EmployeeModel>(LoadEmployeeData());
            DeleteSelectedEmployeesCommand = new RelayCommand(DeleteSelectedEmployees);
        }

        private List<EmployeeModel> LoadEmployeeData()
        {
            return _dataAccess.GetEmployees();
        }

        private void DeleteSelectedEmployees(object parameter)
        {
            var selectedEmployeeIds = GetSelectedEmployeeIdsFromUI(parameter);

            if (selectedEmployeeIds != null && selectedEmployeeIds.Count > 0)
            {
                _dataAccess.DeleteEmployees(selectedEmployeeIds);
                ShowMessage?.Invoke("Selected employees have been deleted.");
                _configurationForm.LoadEmployeeData();
                CloseWindow?.Invoke();
            }
            else
            {
                ShowMessage?.Invoke("Please select at least one employee to delete.");
            }
        }

        private List<int> GetSelectedEmployeeIdsFromUI(object parameter)
        {
            var selectedItems = parameter as IList<object>;
            List<int> selectedIds = new List<int>();

            foreach (var item in selectedItems)
            {
                EmployeeModel employee = item as EmployeeModel;
                if (employee != null)
                {
                    selectedIds.Add(employee.EmployeeID);
                }
            }

            return selectedIds;
        }
    }
}
