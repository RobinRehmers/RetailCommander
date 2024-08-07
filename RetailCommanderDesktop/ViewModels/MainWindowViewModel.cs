using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using RetailCommanderDesktop.Forms;
using RetailCommanderDesktop.Helpers;

namespace RetailCommanderDesktop.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly IConfiguration _config;
        private readonly ITranslationManager _translationManager;
        private readonly ConfigurationFormViewModel _configurationFormViewModel;
        private readonly TranslationLabelUpdater _translationLabelUpdater;

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public double MonthlyTarget { get; set; }
        public double CurrentSales { get; set; }

        public ICommand LoadEmployeesCommand { get; }
        public ICommand OpenConfigurationFormCommand { get; }

        public ConfigurationFormViewModel ConfigurationFormViewModel { get; }

        public IReadOnlyDictionary<string, string> Labels => _translationLabelUpdater.Labels;

        private string _currentCommissionStage;
        private string _nextCommissionStage;
        private double _remainingAmount;
        private double _dailyTarget;
        private string _currentDate;
        private int _remainingDaysInMonth;

        private string _configurationButtonLabel;
        public string ConfigurationButtonLabel
        {
            get => _configurationButtonLabel;
            set
            {
                _configurationButtonLabel = value;
                OnPropertyChanged();
            }
        }

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

        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }

        public int RemainingDaysInMonth
        {
            get => _remainingDaysInMonth;
            set
            {
                _remainingDaysInMonth = value;
                OnPropertyChanged();
            }
        }

        private void UpdateDateInfo()
        {
            CurrentDate = DateTime.Now.ToString("dd.MM.yyyy");
            RemainingDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
        }

        public void LoadCommissionStageInfo()
        {
            LoadMonthlyTarget();
            var stages = _dataAccess.GetCommissionStages();
            var currentStage = stages.OrderByDescending(s => s.TargetAmount).FirstOrDefault(s => s.TargetAmount <= CurrentSales);
            var nextStage = stages.OrderBy(s => s.TargetAmount).FirstOrDefault(s => s.TargetAmount > CurrentSales);

            if (currentStage == null)
            {
                CurrentCommissionStage = "No stage reached.";
                nextStage = stages.OrderBy(s => s.TargetAmount).FirstOrDefault();
            }
            else
            {
                CurrentCommissionStage = $"{currentStage.CommissionPercentage}%";
            }

            if (nextStage != null)
            {
                RemainingAmount = nextStage.TargetAmount - CurrentSales;
                var remainingDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
                DailyTarget = RemainingAmount / remainingDays;
                NextCommissionStage = $"{nextStage.CommissionPercentage}% commission is unlocked at {nextStage.TargetAmount}€ turnover";
            }
            else if (currentStage != null)
            {
                NextCommissionStage = "Highest stage reached.";
                RemainingAmount = 0;
                DailyTarget = 0;
            }
            else
            {
                NextCommissionStage = "No stages available.";
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
            _configurationFormViewModel = ConfigurationFormViewModel;
            LoadEmployeesCommand = new RelayCommand(LoadEmployees);
            OpenConfigurationFormCommand = new RelayCommand(OpenConfigurationForm);

            LoadMonthlyTarget();
            LoadEmployees(null);
            LoadCommissionStageInfo();
            UpdateDateInfo();
            //AddInitialTranslations();

            ConfigurationFormViewModel.PropertyChanged += ConfigurationFormViewModel_PropertyChanged;
            ConfigurationFormViewModel.CalculateAndDistributeCommissions();

            _translationLabelUpdater = new TranslationLabelUpdater(translationManager);
            _translationLabelUpdater.PropertyChanged += TranslationLabelUpdater_PropertyChanged;           
            _translationLabelUpdater.UpdateLabels();

            InitializeTranslations();
        }

        private void InitializeTranslations()
        {
            string selectedLanguage = _config["Language"] ?? "EN";
            _translationManager.LoadTranslations(selectedLanguage);
            _translationLabelUpdater.UpdateLabels();
        }
      
        private void TranslationLabelUpdater_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TranslationLabelUpdater.Labels))
            {
                OnPropertyChanged(nameof(Labels));
            }
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
