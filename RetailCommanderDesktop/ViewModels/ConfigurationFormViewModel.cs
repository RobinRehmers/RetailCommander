using RetailCommanderLibrary.Data;
using RetailCommanderDesktop.Commands;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RetailCommanderLibrary.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ObservableCollection<CommissionStageModel> CommissionStages { get; set; } = new ObservableCollection<CommissionStageModel>();

        public double MonthlyTarget
        {
            get => _monthlyTarget;
            set
            {
                if (_monthlyTarget != value)
                {
                    _monthlyTarget = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SalesProgress));
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
                    OnPropertyChanged(nameof(SalesProgress));
                    UpdateCurrentSalesInDatabase();
                }
            }
        }

        public double SalesProgress
        {
            get
            {
                double progress = _monthlyTarget == 0 ? 0 : (_currentSales / _monthlyTarget) * 100;
                return progress;
            }
        }

        public ConfigurationFormViewModel(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel)
        {
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeForm);
            RemoveEmployeeCommand = new RelayCommand(OpenRemoveEmployeeForm);
            LoadMonthlyTarget();
            LoadCommissionStages();
        }

        public void AddCommissionStage(double targetAmount, double commissionPercentage)
        {
            var newStage = new CommissionStageModel
            {
                TargetAmount = targetAmount,
                CommissionPercentage = commissionPercentage
            };
            CommissionStages.Add(newStage);
            _dataAccess.SaveCommissionStage(newStage);
            OnPropertyChanged(nameof(CommissionStages));
        }

        private void LoadCommissionStages()
        {
            var stages = _dataAccess.GetCommissionStages();
            foreach (var stage in stages)
            {
                CommissionStages.Add(stage);
            }
        }

        public void CalculateAndDistributeCommissions()
        {
            foreach (var stage in CommissionStages.OrderBy(s => s.TargetAmount))
            {
                if (CurrentSales >= stage.TargetAmount)
                {
                    double totalCommission = (CurrentSales * stage.CommissionPercentage) / 100;
                    DistributeCommission(totalCommission);
                }
            }
        }

        private void DistributeCommission(double totalCommission)
        {
            var employees = _dataAccess.GetEmployees();
            double totalHours = employees.Sum(e => e.HoursPerWeek);

            foreach (var employee in employees)
            {
                double employeeShare = (employee.HoursPerWeek / totalHours) * totalCommission;
                employee.Commission += (int)employeeShare;
                _dataAccess.UpdateEmployeeCommission(employee);
            }
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
                OnPropertyChanged(nameof(SalesProgress));
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
