using Microsoft.Extensions.Configuration;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RetailCommanderDesktop
{
    public partial class ConfigurationForm : Window
    {
        private SqliteData _dataAccess;
        private MainWindow _mainWindow;
        private IConfiguration _config;

        public ConfigurationForm(SqliteData dataAccess, MainWindow mainWindow)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindow = mainWindow;
        }

        public ConfigurationForm(IConfiguration config)
        {
            InitializeComponent();
            _config = config;
            _dataAccess = new SqliteData(new SqliteDataAccess(_config));
        }

        /// <summary>
        /// This method is necessary to be able to load the employee data to the grid in real time after adding or removing an employee.
        /// </summary>
        public void LoadEmployeeData()
        {
            _mainWindow.LoadEmployeeData();
        }

        private void AddEmployeeForm_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeForm = new AddEmployeeForm(_dataAccess, this);
            addEmployeeForm.Show();
        }

        private void Removeemployee_Click(object sender, RoutedEventArgs e)
        {
            var DeleteEmployeeForm = new DeleteEmployeeForm(_dataAccess, this);
            DeleteEmployeeForm.ShowDialog();
        }
    }
}
