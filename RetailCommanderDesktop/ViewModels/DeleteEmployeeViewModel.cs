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

        public ObservableCollection<EmployeeViewModel> Employees { get; set; }
        public ICommand DeleteSelectedEmployeesCommand { get; }

        public event Action<string> ShowMessage;
        public event Action CloseWindow;

        /// <summary>
        /// Load the employee data from the database and bind it to the ListBox.
        /// </summary>
        public DeleteEmployeeViewModel(SqliteData dataAccess, ConfigurationForm configurationForm)
        {
            _dataAccess = dataAccess;
            _configurationForm = configurationForm;
            Employees = new ObservableCollection<EmployeeViewModel>(LoadEmployeeData());
            DeleteSelectedEmployeesCommand = new RelayCommand(DeleteSelectedEmployees);
        }

        private List<EmployeeViewModel> LoadEmployeeData()
        {
            var employees = _dataAccess.GetEmployees();
            return employees.Select(e => new EmployeeViewModel { Employee = e, FullName = $"{e.FirstName} {e.LastName}" }).ToList();
        }

        /// <summary>
        /// The DeleteSelectedEmployees_Click method deletes the selected employees from the database. We get the selected employees from the GetSelectedEmployeeIdsFromUI method.
        /// We also update the employees in the main window with the LoadEmployeeData method.
        /// </summary>
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

        /// <summary>
        /// We get the selected employee IDs from the ListBox.
        /// </summary>
        private List<int> GetSelectedEmployeeIdsFromUI(object parameter)
        {
            var selectedItems = parameter as IList<object>;
            List<int> selectedIds = new List<int>();

            foreach (var item in selectedItems)
            {
                EmployeeViewModel employeeViewModel = item as EmployeeViewModel;
                if (employeeViewModel != null)
                {
                    var employee = _dataAccess.GetEmployeeByName(employeeViewModel.Employee.FirstName, employeeViewModel.Employee.LastName);
                    if (employee != null)
                    {
                        selectedIds.Add(employee.EmployeeID);
                    }
                }
            }

            return selectedIds;
        }

        public class EmployeeViewModel
        {
            public EmployeeModel Employee { get; set; }
            public string FullName { get; set; }
        }
    }
}
