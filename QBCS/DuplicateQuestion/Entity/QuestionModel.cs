using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion.Entity
{
    public class QuestionModel
    {
        public string QuestionContent { get; set; }
        public int Hashcode { get; set; }
        public bool IsDuplicate { get; set; }
        public List<OptionModel> Options { get; set; }
        public QuestionModel DuplicateQuestion { get; set; }
    }
}
