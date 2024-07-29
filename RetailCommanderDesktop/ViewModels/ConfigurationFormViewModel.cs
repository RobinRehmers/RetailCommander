using RetailCommanderLibrary.Data;
using RetailCommanderDesktop.Commands;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RetailCommanderLibrary.Models;

namespace RetailCommanderDesktop.ViewModels
{
    public class ConfigurationFormViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private double _monthlyTarget;
        private double _currentSales;

        public ICommand AddEmployeeCommand { get; }
        public ICommand RemoveEmployeeCommand { get; }

        public double MonthlyTarget
        {
            get => _monthlyTarget;
            set
            {
                if (_monthlyTarget != value)
                {
                    _monthlyTarget = value;
                    OnPropertyChanged();
                    UpdateMonthlyTargetInDatabase();
                }
            }
        }

        public double CurrentSales
        {
            get => _currentSales;
            set
            {
                if (_currentSales != value)
                {
                    _currentSales = value;
                    OnPropertyChanged();
                    UpdateCurrentSalesInDatabase();
                }
            }
        }

        public ConfigurationFormViewModel(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel)
        {
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeForm);
            RemoveEmployeeCommand = new RelayCommand(OpenRemoveEmployeeForm);
            LoadMonthlyTarget();
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
            _mainWindowViewModel.LoadEmployees(new object());
        }

        private void LoadMonthlyTarget()
        {
            var monthlyTarget = _dataAccess.GetMonthlyTarget();
            if (monthlyTarget != null)
            {
                MonthlyTarget = monthlyTarget.MonthlyTarget;
                CurrentSales = monthlyTarget.CurrentSales;
            }
        }

        private void UpdateMonthlyTargetInDatabase()
        {
            _dataAccess.UpdateMonthlyTarget(new MonthlyTargetModel
            {
                MonthlyTarget = _monthlyTarget,
                CurrentSales = _currentSales
            });
        }

        private void UpdateCurrentSalesInDatabase()
        {
            _dataAccess.UpdateMonthlyTarget(new MonthlyTargetModel
            {
                MonthlyTarget = _monthlyTarget,
                CurrentSales = _currentSales
            });
        }

        protected new void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
