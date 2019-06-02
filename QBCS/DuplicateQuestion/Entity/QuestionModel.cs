using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion.Entity
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public int Hashcode { get; set; }
        public int DuplicatedQuestionId { get; set; }
        public List<String> RightOptions { get; set; }
        public List<String> WrongOptions { get; set; }
        public int Status { get; set; }

    }
}
