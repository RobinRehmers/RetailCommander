﻿using Microsoft.Extensions.Configuration;
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

namespace RetailCommanderDesktop
{
    public partial class MainWindow : Window
    {
        private SqliteData _dataAccess;
        private IConfiguration _config;

        public MainWindow(IConfiguration config)
        {
            InitializeComponent();
            _config = config;
            _dataAccess = new SqliteData(new SqliteDataAccess(_config));
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMonthlyTarget();
            LoadEmployeeData();
        }

        private void LoadMonthlyTarget()
        {
            var monthlyTarget = _dataAccess.GetMonthlyTarget();

            if (monthlyTarget != null)
            {
                salesProgressBar.Maximum = monthlyTarget.TargetAmount;
                salesProgressBar.Value = monthlyTarget.CurrentSalesAmount;
            }
        }
        private void LoadEmployeeData()
        {
            var employees = _dataAccess.GetEmployees();
            employeeDataGrid.ItemsSource = employees;
        }
    }

}