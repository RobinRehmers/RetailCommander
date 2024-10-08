﻿using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using RetailCommanderDesktop.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RetailCommanderDesktop.Forms;
using RetailCommanderDesktop.Helpers;
using System.ComponentModel;

namespace RetailCommanderDesktop.ViewModels
{
    public class DeleteEmployeeViewModel : BaseViewModel
    {
        private readonly SqliteData _dataAccess;
        private readonly ConfigurationForm _configurationForm;
        private ConfigurationFormViewModel _configurationFormViewModel;
        private readonly ITranslationManager _translationManager;
        private readonly TranslationLabelUpdater _translationLabelUpdater;
        public IReadOnlyDictionary<string, string> Labels => _translationLabelUpdater.Labels;

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ICommand DeleteSelectedEmployeesCommand { get; }

        public event Action<string> ShowMessage;
        public event Action CloseWindow;

        public DeleteEmployeeViewModel(SqliteData dataAccess, ConfigurationForm configurationForm)
        {
            _dataAccess = dataAccess;
            _configurationForm = configurationForm;
            Employees = new ObservableCollection<EmployeeModel>(LoadEmployeeData());
            DeleteSelectedEmployeesCommand = new RelayCommand(DeleteSelectedEmployees);
        }

        public DeleteEmployeeViewModel(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel, ITranslationManager translationManager)
        {
            _dataAccess = dataAccess;
            _configurationFormViewModel = configurationFormViewModel;
            _translationManager = translationManager;

            _translationLabelUpdater = new TranslationLabelUpdater(translationManager);
            _translationLabelUpdater.PropertyChanged += TranslationLabelUpdater_PropertyChanged;
            _translationLabelUpdater.UpdateLabels();

            Employees = new ObservableCollection<EmployeeModel>(LoadEmployeeData());
            DeleteSelectedEmployeesCommand = new RelayCommand(DeleteSelectedEmployees);
        }
        private void TranslationLabelUpdater_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TranslationLabelUpdater.Labels))
            {
                OnPropertyChanged(nameof(Labels));
            }
        }

        private List<EmployeeModel> LoadEmployeeData()
        {
            return _dataAccess.GetEmployees();
        }

        private void DeleteSelectedEmployees(object parameter)
        {
            var selectedEmployeeIds = GetSelectedEmployeeIdsFromUI(parameter);

            if (selectedEmployeeIds != null && selectedEmployeeIds.Count > 0)
            {
                _dataAccess.DeleteEmployees(selectedEmployeeIds);
                ShowMessage?.Invoke("Selected employees have been deleted.");
                Employees.Clear();
                foreach (var employee in LoadEmployeeData())
                {
                    Employees.Add(employee);
                }
                _configurationFormViewModel?.LoadEmployeeData();
                CloseWindow?.Invoke();
            }
            else
            {
                ShowMessage?.Invoke("Please select at least one employee to delete.");
            }
        }

        private List<int> GetSelectedEmployeeIdsFromUI(object parameter)
        {
            var selectedItems = parameter as IList<object>;
            List<int> selectedIds = new List<int>();

            foreach (var item in selectedItems)
            {
                EmployeeModel employee = item as EmployeeModel;
                if (employee != null)
                {
                    selectedIds.Add(employee.EmployeeID);
                }
            }
            return selectedIds;
        }
    }
}
