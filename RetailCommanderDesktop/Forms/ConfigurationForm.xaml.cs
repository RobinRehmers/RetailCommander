using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Models;
using System.Windows;
using System.Windows.Controls;

namespace RetailCommanderDesktop.Forms
{
    public partial class ConfigurationForm : Window
    {
        public ConfigurationForm(SqliteData dataAccess, MainWindowViewModel mainWindowViewModel, ITranslationManager translationManager)
        {
            InitializeComponent();
            DataContext = new ConfigurationFormViewModel(dataAccess, mainWindowViewModel, translationManager);
        }
    }
}
