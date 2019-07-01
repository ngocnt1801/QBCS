using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionHistoryViewModel
    {
        public QuestionViewModel Question { get; set; }
        public List<ExaminationViewModel> Examination { get; set; }
    }
}
