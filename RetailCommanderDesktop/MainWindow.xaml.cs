using Microsoft.Extensions.Configuration;
using RetailCommanderLibrary.Database;
using RetailCommanderLibrary.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RetailCommanderLibrary.Data;
using Microsoft.Extensions.DependencyInjection;

namespace RetailCommanderDesktop
{
    public partial class MainWindow : Window
    {
        private SqliteData _dataAccess;
        private IConfiguration _config;

        /// <summary>
        /// Constructor for the main window.
        /// </summary>
        public MainWindow(IConfiguration config)
        {
            InitializeComponent();
            _config = config;
            _dataAccess = new SqliteData(new SqliteDataAccess(_config));
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// We load the monthly target and employee data from the database when the window is loaded.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMonthlyTarget();
            LoadEmployeeData();
        }

        /// <summary>
        /// We load the monthly target from the database.
        /// </summary>
        private void LoadMonthlyTarget()
        {
            var monthlyTarget = _dataAccess.GetMonthlyTarget();

            if (monthlyTarget != null)
            {
                salesProgressBar.Maximum = monthlyTarget.TargetAmount;
                salesProgressBar.Value = monthlyTarget.CurrentSalesAmount;
            }
        }

        /// <summary>
        /// We load the employee data from the database.
        /// </summary>
        public void LoadEmployeeData()
        {
            var employees = _dataAccess.GetEmployees();
            employeeDataGrid.ItemsSource = employees;
        }

        /// <summary>
        /// We open the configuration form when the configuration button is clicked.
        /// </summary>
        private void ConfigurationForm_Click(object sender, RoutedEventArgs e)
        {
            var configurationForm = new ConfigurationForm(_dataAccess, this);
            configurationForm.Show();
        }

    }
}