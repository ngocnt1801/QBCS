using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class EditQuestionTextboxViewModel
    {
        public string Table { get; set; }
        public int? ImportId { get; set; }
        public int? QuestionId { get; set; }
        public int? CategoryId { get; set; }
        public int? LevelId { get; set; }
        public int? LearningOutcomeId { get; set; }
    }
}
