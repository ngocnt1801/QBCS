using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GenerateExamViewModel
    {
        public int TotalQuestion { get; set; }
        public int EasyPercent { get; set; }
        public int NormalPercent { get; set; }
        public int HardPercent { get; set; }
        public List<string> Topic { get; set; }
        public int EasyQuestion { get; set; }
        public int NormalQuestion { get; set; }
        public int HardQuestion { get; set; }
    }
}
