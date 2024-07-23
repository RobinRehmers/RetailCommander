using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Models
{
    public class SalesModel
    {
        public int SaleID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime SaleDate { get; set; }
        public double SaleAmount { get; set; }
        public string Category { get; set; }
    }
}
