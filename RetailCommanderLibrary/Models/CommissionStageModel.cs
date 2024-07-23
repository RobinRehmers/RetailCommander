using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Models
{
    public class CommissionStageModel
    {
        public int StageID { get; set; }
        public double TargetAmount { get; set; }
        public double CommissionPercentage { get; set; }
    }
}
