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

        public MonthlyTargetModel GetMonthlyTarget()
        {
            string sql = "SELECT * FROM MonthlyTargets LIMIT 1";
            return _db.LoadData<MonthlyTargetModel, dynamic>(sql, new { }, ConnectionStringName).FirstOrDefault();
        }

        public List<EmployeeModel> GetEmployees()
        {
            string sql = "SELECT employeeID, firstName, lastName, hoursPerWeek, commission FROM Employees";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { }, ConnectionStringName);
        }

        public EmployeeModel GetEmployeeByName(string firstName, string lastName)
        {
            string sql = "SELECT employeeID, firstName, lastName, hoursPerWeek, commission FROM Employees WHERE FirstName = @FirstName AND LastName = @LastName";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { FirstName = firstName, LastName = lastName }, ConnectionStringName).FirstOrDefault();
        }

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

        //public List<int> GetEmployeeIdsToDelete()
        //{
        //    string sql = "SELECT Id FROM Employees WHERE /* your condition here */";
        //    return _db.LoadData<int, dynamic>(sql, new { }, ConnectionStringName);
        //}

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
