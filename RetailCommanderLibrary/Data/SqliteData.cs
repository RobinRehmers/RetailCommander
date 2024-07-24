using Dapper;
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
            string sql = "SELECT Name, HoursPerWeek, Commission FROM Employees";
            return _db.LoadData<EmployeeModel, dynamic>(sql, new { }, ConnectionStringName);
        }
    }
}
