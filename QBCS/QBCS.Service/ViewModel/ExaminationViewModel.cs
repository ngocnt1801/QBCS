using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class ExaminationViewModel
    {
        public QuestionViewModel Question { get; set; }
        public TopicViewModel Topic { get; set; }
        public LearningOutcomeViewModel LearningOutcome { get; set; }
        public LevelViewModel Level { get; set; }
    }
}
