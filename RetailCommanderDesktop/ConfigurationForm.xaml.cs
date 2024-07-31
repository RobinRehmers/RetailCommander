using RetailCommanderLibrary.Data;
using RetailCommanderDesktop.ViewModels;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace RetailCommanderDesktop
{
    public partial class ConfigurationForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ITranslationManager _translationManager;

        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = _mainWindowViewModel.ConfigurationFormViewModel;
        }

        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel, ITranslationManager translationManager)
        {
            InitializeComponent();
            _translationManager = translationManager;
            _translationManager.TranslationsUpdated += TranslationManager_TranslationsUpdated;
            DataContext = this;
        }

        private void TranslationManager_TranslationsUpdated(object sender, PropertyChangedEventArgs e)
        {
            // Update UI elements with new translations
            LanguageLabel.Content = _translationManager.GetTranslation("LanguageLabel");
            // Update other UI elements similarly
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLanguage = (string)((ComboBoxItem)LanguageComboBox.SelectedItem).Content;
            _translationManager.LoadTranslations(selectedLanguage);
        }
    }
}
