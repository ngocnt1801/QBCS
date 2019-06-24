using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class PartOfExamViewModel
    {
        public int ExaminationId { get; set; }
        public List<QuestionInExamViewModel> Question { get; set; }
        public int NumberOfQuestion { get; set; }
        public LearningOutcomeViewModel LearningOutcome { get; set; }
    }
}
