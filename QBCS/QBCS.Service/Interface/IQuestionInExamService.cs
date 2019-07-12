﻿using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IQuestionInExamService
    {
        QuestionInExamViewModel GetQuestionInExamById(int questionId);
        int GetCountByLearningOutcome(int learingOutcomeId, int levelId);
    }
}
