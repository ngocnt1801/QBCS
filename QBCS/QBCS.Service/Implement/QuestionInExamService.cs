using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Service.ViewModel;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Entity;

namespace QBCS.Service.Implement
{
    public class QuestionInExamService : IQuestionInExamService
    {
        private IUnitOfWork unitOfWork;
        public QuestionInExamService()
        {
            unitOfWork = new UnitOfWork();
        }

        public int GetCountByLearningOutcome(int learingOutcomeId, int levelId)
        {
            IQueryable<QuestionInExam> questions = unitOfWork.Repository<QuestionInExam>().GetAll();
            List<QuestionInExam> question = questions.Where(q => q.PartOfExamination.LearningOutcomeId == learingOutcomeId && q.LevelId == levelId).ToList();
            if (question == null)
            {
                return 0;
            }
            return question.Count;
        }

        public QuestionInExamViewModel GetQuestionInExamById(int questionId)
        {
            QuestionInExam question = unitOfWork.Repository<QuestionInExam>().GetById(questionId);
            QuestionInExamViewModel questionInExamViewModel = new QuestionInExamViewModel();
            if(question  != null)
            {
                questionInExamViewModel = new QuestionInExamViewModel()
                {
                    Id = question.Id,
                    PartId = question.PartId.HasValue ? (int)question.PartId : 0,
                    QuestionReference = question.QuestionReference.HasValue ? (int)question.QuestionReference : 0,
                    QuestionContent = question.QuestionContent,
                    Priority = question.Priority.HasValue ? (int) question.Priority : 0,
                    Frequency = question.Frequency.HasValue ? (int)question.Frequency : 0,
                    QuestionCode = question.QuestionCode,
                    LevelId = question.LevelId.HasValue ? (int)question.LevelId : 0,
                    Image = question.Image,
                    PartOfExam = new PartOfExamViewModel()
                    {
                        Id = question.PartOfExamination.Id,
                        LearningOutcomeId = question.PartOfExamination.LearningOutcomeId.HasValue ? (int) question.PartOfExamination.LearningOutcomeId : 0,
                        ExaminationId = question.PartOfExamination.ExaminationId.HasValue ? (int)question.PartOfExamination.ExaminationId : 0
                    }
                };
            }
            return questionInExamViewModel;
        }
    }
}
