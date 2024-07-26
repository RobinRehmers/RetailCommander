using Dapper;
using Microsoft.Data.Sqlite;
using RetailCommanderLibrary.Database;
using RetailCommanderLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Data
{
    public class SqliteData
    {
        private const string ConnectionStringName = "SQLiteDb";
        private readonly ISqliteDataAccess _db;

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }

        /// <summary>
        /// We get the monthly target data from the database.
        /// </summary>
        public MonthlyTargetModel GetMonthlyTarget()
        {
            string sql = "SELECT * FROM MonthlyTargets LIMIT 1";
            return _db.LoadData<MonthlyTargetModel, dynamic>(sql, new { }, ConnectionStringName).FirstOrDefault();
        }

        /// <summary>
        /// We get the actual employee data from the database.
        /// </summary>
        public List<EmployeeModel> GetEmployees()
        {
            string sql = "SELECT employeeID, firstName, lastName, hoursPerWeek, commission FROM Employees";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { }, ConnectionStringName);
        }

        /// <summary>
        /// We get the employee data by name from the database. This is used by the GetSelectedEmployeeIdsFromUI method in the MainWindow.xaml.cs file.
        /// </summary>
        public EmployeeModel GetEmployeeByName(string firstName, string lastName)
        {
            string sql = "SELECT employeeID, firstName, lastName, hoursPerWeek, commission FROM Employees WHERE FirstName = @FirstName AND LastName = @LastName";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { FirstName = firstName, LastName = lastName }, ConnectionStringName).FirstOrDefault();
        }

        /// <summary>
        /// We check if the employee exists in the database. If the employee does not exist, we add the employee data.
        /// </summary>
        public void AddEmployee(string firstName, string lastName, int hoursPerWeek, int commission)
        {

            string sql = @"select 1 from Employees where firstName = @firstName and lastName = @lastName;";
            int results = _db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName },
                                                                       ConnectionStringName).Count();

            if (results == 0)
            {
                sql = @"insert into Employees (firstName, lastName, hoursPerWeek, commission)
                  values (@firstName, @lastName, @hoursPerWeek,@commission);";
                _db.SaveData(sql, new { firstName, lastName, hoursPerWeek, commission }, ConnectionStringName);
            }
            else
            {
                throw new Exception("Employee already exists");
            }
        }

        /// <summary>
        /// This function is used to delete employees from the database, after the user has selected them in the DeleteEmployeeForm.
        /// </summary>
        public void DeleteEmployees(List<int> employeeIds)
        {
            foreach (var employeeId in employeeIds)
            {
                string sql = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                _db.SaveData(sql, new { EmployeeID = employeeId }, ConnectionStringName);
            }
        }
    }
 }
