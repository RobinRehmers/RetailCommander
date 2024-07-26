using RetailCommanderLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RetailCommanderLibrary.Models;
using RetailCommanderLibrary.Database;

namespace RetailCommanderDesktop
{
    public partial class DeleteEmployeeForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindow _mainWindow;

        public DeleteEmployeeForm(SqliteData dataAccess, MainWindow mainWindow)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindow = mainWindow;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            var employees = _dataAccess.GetEmployees();
            employeeListBox.ItemsSource = employees.Select(e => new EmployeeViewModel { Employee = e, FullName = $"{e.FirstName} {e.LastName}" }).ToList();
        }

        private void DeleteSelectedEmployees_Click(object sender, RoutedEventArgs e)
        {
            List<int> selectedEmployeeIds = GetSelectedEmployeeIdsFromUI();

            if (selectedEmployeeIds != null && selectedEmployeeIds.Count > 0)
            {
                _dataAccess.DeleteEmployees(selectedEmployeeIds);
                MessageBox.Show("Selected employees have been deleted.");
                _mainWindow.LoadEmployeeData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie mindestens einen Mitarbeiter zum Löschen aus.");
            }
        }

        private List<int> GetSelectedEmployeeIdsFromUI()
        {
            List<int> selectedIds = new List<int>();

            foreach (var item in employeeListBox.SelectedItems)
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
    }

    public class EmployeeViewModel
    {
        public EmployeeModel Employee { get; set; }
        public string FullName { get; set; }
    }
}
