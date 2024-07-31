using RetailCommanderLibrary.Data;
using RetailCommanderDesktop.ViewModels;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;

namespace RetailCommanderDesktop
{
    public partial class ConfigurationForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ITranslationManager _translationManager;

        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel, ITranslationManager translationManager)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            _translationManager = translationManager;
            _translationManager.TranslationsUpdated += TranslationManager_TranslationsUpdated;
            DataContext = _mainWindowViewModel.ConfigurationFormViewModel;

            var config = (IConfiguration)Application.Current.Resources["AppConfig"];
            var initialLanguage = config["Language"];
            _translationManager.LoadTranslations(initialLanguage);
            SetLanguageComboBox(initialLanguage);

            LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
        }

        private void TranslationManager_TranslationsUpdated(object sender, PropertyChangedEventArgs e)
        {
            LanguageLabel.Content = _translationManager.GetTranslation("LanguageLabel");
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_translationManager == null) return;

            var selectedLanguage = (string)((ComboBoxItem)LanguageComboBox.SelectedItem).Content;
            _translationManager.LoadTranslations(selectedLanguage);


            var config = (IConfiguration)Application.Current.Resources["AppConfig"];
            config["Language"] = selectedLanguage;
        }

        private void SetLanguageComboBox(string language)
        {
            foreach (ComboBoxItem item in LanguageComboBox.Items)
            {
                if ((string)item.Content == language)
                {
                    LanguageComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
