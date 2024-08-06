using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RetailCommanderDesktop.Forms
{
    public partial class ConfigurationForm : Window
    {
        private readonly SqliteData _dataAccess;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel, ITranslationManager translationManager)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindowViewModel = mainWindowViewModel;
           // _mainWindowViewModel.ConfigurationFormViewModel.SelectedLanguage = "EN"; 
           //_mainWindowViewModel.ConfigurationFormViewModel.PropertyChanged += ConfigurationFormViewModel_PropertyChanged;


            DataContext = _mainWindowViewModel.ConfigurationFormViewModel;
        }

        private void ConfigurationFormViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConfigurationFormViewModel.LanguageLabelText))
            {
                LanguageLabel.Content = _mainWindowViewModel.ConfigurationFormViewModel.LanguageLabelText;
            }
        }
    }
}
