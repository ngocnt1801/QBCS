using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBCS.Web.Models
{
    public class QuestionDetailViewModel
    {
        public QuestionViewModel QuestionViewModel { get; set; }

        public List<Topic> Topics { get; set; }

        public List<Level> Levels { get; set; }

        public List<LearningOutcome> LearningOutcomes { get; set; }
    }
}