using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GenerateExamViewModel
    {
        public int ExamId { get; set; }
        public int TotalQuestion { get; set; }
        public int EasyPercent { get; set; }
        public int MediumPercent { get; set; }
        public int HardPercent { get; set; }
        public List<string> Topic { get; set; }
        public int EasyQuestion { get; set; }
        public int MediumQuestion { get; set; }
        public int HardQuestion { get; set; }
        public int CategoryId { get; set; }
    }
}
