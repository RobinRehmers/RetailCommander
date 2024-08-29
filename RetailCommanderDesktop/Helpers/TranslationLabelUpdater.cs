using RetailCommanderLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderDesktop.Helpers
{
    public class TranslationLabelUpdater : INotifyPropertyChanged
    {
        private readonly ITranslationManager _translationManager;
        private readonly Dictionary<string, string> _labels;

        public TranslationLabelUpdater(ITranslationManager translationManager)
        {
            _translationManager = translationManager;
            _labels = new Dictionary<string, string>();
            _translationManager.TranslationsUpdated += (s, e) => UpdateLabels();
            UpdateLabels();
        }

        public IReadOnlyDictionary<string, string> Labels => _labels;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateLabels()
        {                              
            //mainwindow
            _labels["ConfigurationBtn"] = _translationManager.GetTranslation("ConfigurationBtn");
            _labels["RemainingDaysInMonth"] = _translationManager.GetTranslation("RemainingDaysInMonth");
            _labels["CurrentCommissionStage"] = _translationManager.GetTranslation("CurrentCommissionStage");
            _labels["NextCommissionStage"] = _translationManager.GetTranslation("NextCommissionStage");
            _labels["RemainingAmount"] = _translationManager.GetTranslation("RemainingAmount");
            _labels["DailyTarget"] = _translationManager.GetTranslation("DailyTarget");

            //mainwindow grid
            _labels["FN"] = _translationManager.GetTranslation("FN");
            _labels["LastName"] = _translationManager.GetTranslation("LastName");
            _labels["HoursPerWeek"] = _translationManager.GetTranslation("HoursPerWeek");
            _labels["Commission"] = _translationManager.GetTranslation("Commission");

            //configwindow
            _labels["EmployeeGoalConfiguration"] = _translationManager.GetTranslation("EmployeeGoalConfiguration");
            _labels["CommissionStages"] = _translationManager.GetTranslation("CommissionStages");
            _labels["MonthlyTarget"] = _translationManager.GetTranslation("MonthlyTarget");
            _labels["CurrentSales"] = _translationManager.GetTranslation("CurrentSales");
            _labels["NewCommissionStageTarget"] = _translationManager.GetTranslation("NewCommissionStageTarget");
            _labels["NewCommissionStagePercentage"] = _translationManager.GetTranslation("NewCommissionStagePercentage");
            _labels["AddEmployee"] = _translationManager.GetTranslation("AddEmployee");
            _labels["DeleteEmployee"] = _translationManager.GetTranslation("DeleteEmployee");
            _labels["AddCommissionStage"] = _translationManager.GetTranslation("AddCommissionStage");
            _labels["DeleteSelectedCommissionStage"] = _translationManager.GetTranslation("DeleteSelectedCommissionStage");

            // addemployeeform
            _labels["FirstName"] = _translationManager.GetTranslation("FirstName");
            _labels["LastName"] = _translationManager.GetTranslation("LastName");
            _labels["HoursPerWeek"] = _translationManager.GetTranslation("HoursPerWeek");
            _labels["AddNewEmployee"] = _translationManager.GetTranslation("AddNewEmployee");

            OnPropertyChanged(nameof(Labels));
        }
    }
}
