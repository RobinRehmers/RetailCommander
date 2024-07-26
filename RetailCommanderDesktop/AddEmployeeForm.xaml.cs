using RetailCommanderLibrary.Data;
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
        private readonly SqliteData _dataAccess;
        private readonly MainWindow _mainWindow;
        public AddEmployeeForm(SqliteData dataAccess, MainWindow mainWindow)
        {
            InitializeComponent();
            _dataAccess = dataAccess;
            _mainWindow = mainWindow;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

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
                _mainWindow.LoadEmployeeData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}");
            }
        }
    }
}
