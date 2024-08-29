using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using System.Windows;

namespace RetailCommanderDesktop.Forms
{
    public partial class AddEmployeeForm : Window
    {
        private readonly ITranslationManager _translationManager;
        public AddEmployeeForm(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel, ITranslationManager translationManager)
        {
            InitializeComponent();
            _translationManager = translationManager;
            DataContext = new AddEmployeeFormViewModel(dataAccess, configurationFormViewModel, _translationManager);
        }
    }
}
