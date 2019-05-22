using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBCS.Web.Models
{
    public class UpdateQuestionModel
    {
        public int QuestionId { get; set; }

        public string QuestionContent { get; set; }

        public int? TopicId { get; set; }

        public int? LearningOutcomeId { get; set; }

        public int? LevelId { get; set; }

        public int OptionId { get; set; }

        public string OptionContent { get; set; }

        public bool? IsCorrect { get; set; }
    }
}