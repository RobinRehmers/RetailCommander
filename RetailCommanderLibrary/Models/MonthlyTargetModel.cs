using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Models
{
    public class MonthlyTargetModel
    {
        public string MonthYear { get; set; }
        public double TargetAmount { get; set; }
        public double CurrentSalesAmount { get; set; }
    }
}
