using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.ViewModel
{
    public class ExaminationStatisticViewModel
    {
        public List<ExaminationChartViewModel> Chart { get; set; }
        public List<ExamStatTableViewModel> Question { get; set; }
    }
}
