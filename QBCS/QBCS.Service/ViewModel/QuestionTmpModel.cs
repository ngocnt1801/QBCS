using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionTmpModel
    {
        public string Code { get; set; }
        public string QuestionContent { get; set; }
        public List<OptionTemp> Options { get; set; }
        public int Status { get; set; }
        public int DuplicatedId { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public string LearningOutcome { get; set; }
        public string Level { get; set; }
    }
}
