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
        public string QuestionCode { get; set; }
        public int Hashcode { get; set; }
        public string Test { get; set; }
        public int? DuplicatedQuestionId { get; set; }
        public int? DuplicatedWithImportId { get; set; }
        public List<OptionModel> Options { get; set; }
        public int Status { get; set; }
        public bool IsBank { get; set; }
    }
}
