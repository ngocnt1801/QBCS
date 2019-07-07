using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionInExamViewModel
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public int QuestionReference { get; set; }
        public string QuestionContent { get; set; }
        public int LevelId { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public int Priority { get; set; }
        public string QuestionCode { get; set; }
        public string Image { get; set; }
        public int Frequency { get; set; }
        public LevelViewModel Level { get; set; }
        public string LearningOutcome { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public CategoryViewModel Category { get; set; }
        public ExaminationViewModel Examination { get; set; }
    }
}
