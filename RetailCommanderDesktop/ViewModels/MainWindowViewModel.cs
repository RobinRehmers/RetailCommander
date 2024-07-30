using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

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

        public ConfigurationFormViewModel ConfigurationFormViewModel { get; }

        public MainWindowViewModel(SqliteData dataAccess, IConfiguration config)
        {
            _dataAccess = dataAccess;
            _config = config;
            ConfigurationFormViewModel = new ConfigurationFormViewModel(_dataAccess, this);
            Employees = new ObservableCollection<EmployeeModel>();
            LoadEmployeesCommand = new RelayCommand(LoadEmployees);
            OpenConfigurationFormCommand = new RelayCommand(OpenConfigurationForm);
            LoadMonthlyTarget();
            LoadEmployees(null);

           ConfigurationFormViewModel.PropertyChanged += ConfigurationFormViewModel_PropertyChanged;
        }

        private void ConfigurationFormViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConfigurationFormViewModel.SalesProgress))
            {
                OnPropertyChanged(nameof(SalesProgress));
            }
        }

        public void LoadEmployees(object parameter)
        {
            if (Employees == null)
            {
                Employees = new ObservableCollection<EmployeeModel>();
            }
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
                MonthlyTarget = monthlyTarget.MonthlyTarget;
                CurrentSales = monthlyTarget.CurrentSales;
                OnPropertyChanged(nameof(MonthlyTarget));
                OnPropertyChanged(nameof(CurrentSales));
                OnPropertyChanged(nameof(SalesProgress));
            }
        }

        private void OpenConfigurationForm(object parameter)
        {
            var configurationForm = new ConfigurationForm(_dataAccess, this);
            configurationForm.ShowDialog();
        }

        public double SalesProgress => ConfigurationFormViewModel.SalesProgress;
    }
}
