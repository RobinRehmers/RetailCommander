using RetailCommanderDesktop.ViewModels;
using RetailCommanderLibrary.Data;
using System.Windows;

namespace RetailCommanderDesktop.Forms
{
    public partial class AddEmployeeForm : Window
    {
        public AddEmployeeForm(SqliteData dataAccess, ConfigurationFormViewModel configurationFormViewModel)
        {
            InitializeComponent();
            DataContext = new AddEmployeeFormViewModel(dataAccess, configurationFormViewModel);
        }
    }
}
