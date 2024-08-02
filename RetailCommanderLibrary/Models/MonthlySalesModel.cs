using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Models
{
    public class MonthlySalesModel
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public double Sales { get; set; }
    }
}
