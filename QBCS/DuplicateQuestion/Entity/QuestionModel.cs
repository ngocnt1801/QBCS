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
        public int CourseId { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionCode { get; set; }
        public int Hashcode { get; set; }
        public string Test { get; set; }
        public int? DuplicatedQuestionId { get; set; }
        public int? DuplicatedWithImportId { get; set; }
        public List<OptionModel> Options { get; set; }
        public int Status { get; set; }
        public bool IsBank { get; set; }
        public string Category { get; set; }
        public string LearningOutcome { get; set; }
        public string Level { get; set; }
        public int? CategoryId { get; set; }
        public int? LearningOutcomeId { get; set; }
        public int? LevelId { get; set; }
        public int? UpdateQuestionId { get; set; }
        public string Image { get; set; }
        public bool IsNotImage { get; set; }
        public int QuestionId { get; set; }
        public string Result { get; set; }
        public List<ImageModel> Images { get; set; }
        public string SkipQuestions { get; set; }

        public override string ToString()
        {
            return Id + "-" + IsBank + "-" + Result;
        }
    }
}
