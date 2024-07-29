using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;

namespace RetailCommanderDesktop.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly IConfiguration _config;

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public double MonthlyTarget { get; set; }
        public double CurrentSales { get; set; }
        public ICommand LoadEmployeesCommand { get; }
        public ICommand OpenConfigurationFormCommand { get; }

        public MainWindowViewModel(SqliteData dataAccess, IConfiguration config)
        {
            _dataAccess = dataAccess;
            _config = config;
            Employees = new ObservableCollection<EmployeeModel>();
            LoadEmployeesCommand = new RelayCommand(LoadEmployees);
            OpenConfigurationFormCommand = new RelayCommand(OpenConfigurationForm);
            LoadMonthlyTarget();
            LoadEmployees(null);
        }

        public void LoadEmployees(object parameter)
        {
            Employees.Clear();
            var employees = _dataAccess.GetEmployees();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private void LoadMonthlyTarget()
        {
            var monthlyTarget = _dataAccess.GetMonthlyTarget();
            if (monthlyTarget != null)
            {
                MonthlyTarget = monthlyTarget.TargetAmount;
                CurrentSales = monthlyTarget.CurrentSalesAmount;
                OnPropertyChanged(nameof(MonthlyTarget));
                OnPropertyChanged(nameof(CurrentSales));
            }
        }

        private void OpenConfigurationForm(object parameter)
        {
            var configurationForm = new ConfigurationForm(_dataAccess, this);
            configurationForm.Show();
        }
    }
}
