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
    public partial class AddEmployeeForm : Window
    {
        private SqliteData _dataAccess;
        private ConfigurationForm _configurationForm;
        private IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the AddEmployeeForm class with the provided SqliteData and ConfigurationForm instances.
        /// This constructor is used when the AddEmployeeForm is created from within an existing ConfigurationForm. 
        /// This allows the ConfigurationForm to be updated after adding an employee by calling the LoadEmployeeData method.
        /// </summary>
        public AddEmployeeForm(SqliteData dataAccess, ConfigurationForm configurationForm)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _configurationForm = configurationForm;
        }

        /// <summary>
        /// Initializes a new instance of the AddEmployeeForm class with the provided IConfiguration instance.
        /// This constructor is used when the AddEmployeeForm is created independently, without an existing ConfigurationForm. 
        /// The constructor initializes the data access layer (SqliteData) using the configuration settings.
        /// </summary>
        public AddEmployeeForm(IConfiguration config)
        {
            InitializeComponent();
            _config = config;
            _dataAccess = new SqliteData(new SqliteDataAccess(_config));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Adds an employee to the database.
        /// </summary>
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = firstnamebox.Text;
                string lastName = lastnamebox.Text;
                int hours = int.Parse(hoursbox.Text);
                int commission = 0;

                _dataAccess.AddEmployee(firstName, lastName, hours, commission);
                MessageBox.Show("Employee added successfully!");
                _configurationForm.LoadEmployeeData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}");
            }
        }
    }
}
