using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
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

        public Dictionary<string, string> LoadTranslations(string language)
        {
            string sql = "SELECT ControlName, Text FROM Translations WHERE Language = @Language";
            var result = _db.LoadData<(string ControlName, string Text), dynamic>(sql, new { Language = language }, ConnectionStringName);
            return result.ToDictionary(x => x.ControlName, x => x.Text);
        }

        public void SaveTranslation(string controlName, string language, string text)
        {
            string sql = "INSERT INTO Translations (ControlName, Language, Text) VALUES (@ControlName, @Language, @Text)";
            _db.SaveData(sql, new { ControlName = controlName, Language = language, Text = text }, ConnectionStringName);
        }

        public void DeleteCommissionStage(CommissionStageModel stage)
        {
            if (stage != null)
            {
            string sql = "DELETE FROM CommissionStages WHERE StageID = @StageID;";
            _db.SaveData(sql, new { StageID = stage.StageID }, ConnectionStringName);
            }
        }

        public void SaveCommissionStage(CommissionStageModel stage)
        {
            string sql = @"INSERT INTO CommissionStages (TargetAmount, CommissionPercentage)
                       VALUES (@TargetAmount, @CommissionPercentage);";
            _db.SaveData(sql, new { stage.TargetAmount, stage.CommissionPercentage }, ConnectionStringName);
        }

        public List<CommissionStageModel> GetCommissionStages()
        {
            string sql = "SELECT * FROM CommissionStages;";
            return _db.LoadData<CommissionStageModel, dynamic>(sql, new { }, ConnectionStringName).ToList();
        }

        public List<EmployeeModel> GetEmployees()
        {
            string sql = "SELECT * FROM Employees;";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { }, ConnectionStringName).ToList();
        }

        public void UpdateEmployeeCommission(EmployeeModel employee)
        {
            string sql = @"UPDATE Employees SET Commission = @Commission WHERE EmployeeID = @EmployeeID;";
            _db.SaveData(sql, new { employee.Commission, employee.EmployeeID }, ConnectionStringName);
        }

        public MonthlyTargetModel GetMonthlyTarget()
        {
            string sql = "SELECT * FROM MonthlyTargets LIMIT 1";
            return _db.LoadData<MonthlyTargetModel, dynamic>(sql, new { }, ConnectionStringName).FirstOrDefault();
        }

        public void UpdateMonthlyTarget(MonthlyTargetModel monthlyTarget)
        {
            string sql = @"UPDATE MonthlyTargets 
                           SET MonthlyTarget = @MonthlyTarget, 
                               CurrentSales = @CurrentSales
                           WHERE Id = 1";

            _db.SaveData(sql, monthlyTarget, ConnectionStringName);
        }

        /// <summary>
        /// We get the employee data by name from the database. This is used by the GetSelectedEmployeeIdsFromUI method in the MainWindow.xaml.cs file.
        /// </summary>
        public EmployeeModel GetEmployeeByName(string firstName, string lastName)
        {
            string sql = "SELECT employeeID, firstName, lastName, hoursPerWeek, commission FROM Employees WHERE FirstName = @FirstName AND LastName = @LastName";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { FirstName = firstName, LastName = lastName }, ConnectionStringName).FirstOrDefault();
        }

        public void AddEmployee(string firstName, string lastName, int hoursPerWeek, int commission)
        {
            string sql = @"select 1 from Employees where firstName = @firstName and lastName = @lastName;";
            int results = _db.LoadData<dynamic, dynamic>(sql, new { firstName, lastName }, ConnectionStringName).Count();

            if (results == 0)
            {
                sql = @"insert into Employees (firstName, lastName, hoursPerWeek, commission)
                        values (@firstName, @lastName, @hoursPerWeek, @commission);";
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
