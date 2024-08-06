using RetailCommanderDesktop.Commands;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using RetailCommanderDesktop.Forms;
using System.Windows;

namespace RetailCommanderDesktop.ViewModels
{
    public class ConfigurationFormViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ITranslationManager _translationManager;

        private CommissionStageModel _selectedCommissionStage;
        private double _monthlyTarget;
        private double _currentSales;
        private double _newTargetAmount;
        private double _newCommissionPercentage;
        private string _languageLabel;
        private string _selectedLanguage;

        public ObservableCollection<CommissionStageModel> CommissionStages { get; set; } = new ObservableCollection<CommissionStageModel>();
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string>();

        public ICommand AddEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand AddCommissionStageCommand { get; }
        public ICommand DeleteCommissionStageCommand { get; }
        public ICommand LanguageChangedCommand { get; }

        public ConfigurationFormViewModel(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel, ITranslationManager translationManager)
        {
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            _translationManager = translationManager;

            AddEmployeeCommand = new RelayCommand(OpenAddEmployeeForm);
            DeleteEmployeeCommand = new RelayCommand(OpenDeleteEmployeeForm);
            AddCommissionStageCommand = new RelayCommand(AddCommissionStage);
            DeleteCommissionStageCommand = new RelayCommand(DeleteCommissionStage, CanDeleteCommissionStage);
            LanguageChangedCommand = new RelayCommand<string>(OnLanguageChanged);

            LoadMonthlyTarget();
            LoadCommissionStages();
            InitializeTranslations();

            Languages.Add("EN");
            Languages.Add("DE");

            CalculateAndDistributeCommissions();
        }

        public CommissionStageModel SelectedCommissionStage
        {
            get => _selectedCommissionStage;
            set
            {
                _selectedCommissionStage = value;
                OnPropertyChanged();
            }
        }

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
                    CalculateAndDistributeCommissions();
                    _mainWindowViewModel.LoadCommissionStageInfo();
                }
            }
        }

        public double NewTargetAmount
        {
            get => _newTargetAmount;
            set
            {
                if (_newTargetAmount != value)
                {
                    _newTargetAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public double NewCommissionPercentage
        {
            get => _newCommissionPercentage;
            set
            {
                if (_newCommissionPercentage != value)
                {
                    _newCommissionPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LanguageLabel
        {
            get => _languageLabel;
            set
            {
                _languageLabel = value;
                OnPropertyChanged();
            }
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                    OnLanguageChanged(_selectedLanguage);
                    _translationManager.LoadTranslations(_selectedLanguage);
                    SaveLanguageSetting(_selectedLanguage);
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

        private void InitializeTranslations()
        {
            var config = (IConfiguration)Application.Current.Resources["AppConfig"];
            SelectedLanguage = config["Language"];
            _translationManager.LoadTranslations(SelectedLanguage);
        }

        public void OnLanguageChanged(string selectedLanguage)
        {
            _translationManager.LoadTranslations(selectedLanguage);
            SaveLanguageSetting(selectedLanguage);
            LanguageLabel = _translationManager.GetTranslation("LanguageLabel");
        }

        private void SaveLanguageSetting(string language)
        {
            var config = (IConfiguration)Application.Current.Resources["AppConfig"];
            config["Language"] = language;
        }


        private void TranslationManager_TranslationsUpdated(object sender, PropertyChangedEventArgs e)
        {
            LanguageLabel = _translationManager.GetTranslation("LanguageLabel");
        }


        private void DeleteCommissionStage(object parameter)
        {
            if (SelectedCommissionStage != null)
            {
                _dataAccess.DeleteCommissionStage(SelectedCommissionStage);
                CommissionStages.Remove(SelectedCommissionStage);
                OnPropertyChanged(nameof(CommissionStages));
                CalculateAndDistributeCommissions();
            }
        }

        private bool CanDeleteCommissionStage(object parameter)
        {
            return SelectedCommissionStage != null;
        }

        public void AddCommissionStage(object parameter)
        {
            var newStage = new CommissionStageModel
            {
                TargetAmount = NewTargetAmount,
                CommissionPercentage = NewCommissionPercentage
            };
            CommissionStages.Add(newStage);
            _dataAccess.SaveCommissionStage(newStage);

            CommissionStages = new ObservableCollection<CommissionStageModel>(CommissionStages.OrderBy(s => s.TargetAmount));
            OnPropertyChanged(nameof(CommissionStages));
            CalculateAndDistributeCommissions();
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
            var activeStage = CommissionStages
                .OrderByDescending(s => s.TargetAmount)
                .FirstOrDefault(s => CurrentSales >= s.TargetAmount);

            if (activeStage != null)
            {
                double totalCommission = (CurrentSales * activeStage.CommissionPercentage) / 100;
                DistributeCommission(totalCommission);
            }
            else
            {
                ResetCommissions();
            }

            _mainWindowViewModel.LoadEmployees(null);
            _mainWindowViewModel.LoadCommissionStageInfo();
        }

        private void DistributeCommission(double totalCommission)
        {
            var employees = _dataAccess.GetEmployees();
            double totalHours = employees.Sum(e => e.HoursPerWeek);

            foreach (var employee in employees)
            {
                double employeeShare = (employee.HoursPerWeek / totalHours) * totalCommission;
                employee.Commission = (int)employeeShare;
                _dataAccess.UpdateEmployeeCommission(employee);
            }
        }

        private void ResetCommissions()
        {
            var employees = _dataAccess.GetEmployees();

            foreach (var employee in employees)
            {
                employee.Commission = 0;
                _dataAccess.UpdateEmployeeCommission(employee);
            }
        }

        private void OpenAddEmployeeForm(object parameter)
        {
            var addEmployeeForm = new AddEmployeeForm(_dataAccess, this);
            addEmployeeForm.Show();
        }

        private void OpenDeleteEmployeeForm(object parameter)
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
    }
}
