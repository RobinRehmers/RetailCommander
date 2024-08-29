using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System;
using System.Windows.Input;
using RetailCommanderDesktop.Forms;
using System.Windows;
using RetailCommanderDesktop.Helpers;
using System.ComponentModel;

namespace RetailCommanderDesktop.ViewModels
{
    public class AddEmployeeFormViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationFormViewModel _configurationFormViewModel;
        private readonly ITranslationManager _translationManager;
        private readonly TranslationLabelUpdater _translationLabelUpdater;
        public IReadOnlyDictionary<string, string> Labels => _translationLabelUpdater.Labels;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Hours { get; set; }

        public ICommand AddEmployeeCommand { get; }

        public event Action<string> ShowMessage;
        public event Action CloseWindow;

        public AddEmployeeFormViewModel(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel, ITranslationManager translationManager)
        {
            _dataAccess = dataAccess;
            _configurationFormViewModel = configurationFormViewModel;
            _translationManager = translationManager;
            AddEmployeeCommand = new RelayCommand(AddEmployee);

            _translationLabelUpdater = new TranslationLabelUpdater(translationManager);
            _translationLabelUpdater.PropertyChanged += TranslationLabelUpdater_PropertyChanged;
            _translationLabelUpdater.UpdateLabels();

            ShowMessage += OnShowMessage;
            CloseWindow += OnCloseWindow;
        }
        private void TranslationLabelUpdater_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TranslationLabelUpdater.Labels))
            {
                OnPropertyChanged(nameof(Labels));
            }
        }

        private void AddEmployee(object parameter)
        {
            try
            {
                _dataAccess.AddEmployee(FirstName, LastName, Hours, 0);
                ShowMessage?.Invoke("Employee added successfully!");
                _configurationFormViewModel.LoadEmployeeData();
                _configurationFormViewModel.CalculateAndDistributeCommissions();
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                ShowMessage?.Invoke($"Error adding employee: {ex.Message}");
            }
        }

        private void OnShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void OnCloseWindow()
        {
            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}
