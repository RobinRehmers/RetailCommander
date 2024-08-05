using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using RetailCommanderDesktop.Forms;

namespace RetailCommanderDesktop.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly IConfiguration _config;
        private readonly ITranslationManager _translationManager;

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public double MonthlyTarget { get; set; }
        public double CurrentSales { get; set; }

        public ICommand LoadEmployeesCommand { get; }
        public ICommand OpenConfigurationFormCommand { get; }

        public ConfigurationFormViewModel ConfigurationFormViewModel { get; }


        private string _currentCommissionStage;
        private string _nextCommissionStage;
        private double _remainingAmount;
        private double _dailyTarget;

        public string CurrentCommissionStage
        {
            get => _currentCommissionStage;
            set
            {
                _currentCommissionStage = value;
                OnPropertyChanged();
            }
        }

        public string NextCommissionStage
        {
            get => _nextCommissionStage;
            set
            {
                _nextCommissionStage = value;
                OnPropertyChanged();
            }
        }

        public double RemainingAmount
        {
            get => _remainingAmount;
            set
            {
                _remainingAmount = value;
                OnPropertyChanged();
            }
        }

        public double DailyTarget
        {
            get => _dailyTarget;
            set
            {
                _dailyTarget = value;
                OnPropertyChanged();
            }
        }

        public void LoadCommissionStageInfo()
        {
            var stages = _dataAccess.GetCommissionStages();
            var currentStage = stages.OrderByDescending(s => s.TargetAmount).FirstOrDefault(s => s.TargetAmount <= CurrentSales);
            var nextStage = stages.OrderBy(s => s.TargetAmount).FirstOrDefault(s => s.TargetAmount > CurrentSales);

            if (currentStage == null)
            {
                CurrentCommissionStage = "Aktuelle Provisionsstufe: Keine Stufe erreicht.";
                NextCommissionStage = string.Empty;
                RemainingAmount = 0;
                DailyTarget = 0;
                return;
            }

            CurrentCommissionStage = $"Aktuelle Provisionsstufe: {currentStage.CommissionPercentage}%";

            if (nextStage != null)
            {
                RemainingAmount = nextStage.TargetAmount - CurrentSales;
                var remainingDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
                DailyTarget = RemainingAmount / remainingDays;

                NextCommissionStage = $"Nächste Provisionsstufe: {nextStage.CommissionPercentage}% Provision werden bei {nextStage.TargetAmount}€ Umsatz freigeschaltet";
            }
            else
            {
                NextCommissionStage = "Höchste Stufe erreicht.";
                RemainingAmount = 0;
                DailyTarget = 0;
            }
        }

        public MainWindowViewModel(SqliteData dataAccess, IConfiguration config, ITranslationManager translationManager)
        {
            _dataAccess = dataAccess;
            _config = config;
            _translationManager = translationManager;

            ConfigurationFormViewModel = new ConfigurationFormViewModel(_dataAccess, this, _translationManager);
         
            LoadEmployeesCommand = new RelayCommand(LoadEmployees);
            OpenConfigurationFormCommand = new RelayCommand(OpenConfigurationForm);

            LoadMonthlyTarget();
            LoadEmployees(null);
            LoadCommissionStageInfo();
            //AddInitialTranslations();

            ConfigurationFormViewModel.PropertyChanged += ConfigurationFormViewModel_PropertyChanged;
            ConfigurationFormViewModel.CalculateAndDistributeCommissions();
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
                //OnPropertyChanged(nameof(SalesProgress));
            }
        }

        private void OpenConfigurationForm(object parameter)
        {
            var configurationForm = new ConfigurationForm(_dataAccess, this, _translationManager);
            configurationForm.ShowDialog();
        }

        private void AddInitialTranslations()
        {
            //_translationManager.SaveTranslation("LanguageLabel", "DE", "Sprache");
        }

        public double SalesProgress => ConfigurationFormViewModel.SalesProgress;
    }
}
