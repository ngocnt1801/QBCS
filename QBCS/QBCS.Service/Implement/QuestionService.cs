using QBCS.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Service.ViewModel;

namespace QBCS.Service.Implement
{
    public class QuestionService : IQuestionService
    {
        private IUnitOfWork unitOfWork;

        public QuestionService()
        {
            unitOfWork = new UnitOfWork();
        }

        public bool Add(QuestionViewModel question)
        {
            bool result = false;
            if (!IsDuplicateQuestion(question))
            {
                var entity = new Question()
                {
                    QuestionContent = question.QuestionContent,
                    CourseId = question.CourseId,
                    IsDisable = false,
                    Options = question.Options.Select(o => new Option()
                    {
                        IsCorrect = o.IsCorrect,
                        OptionContent = o.OptionContent
                    }).ToList(),
                    Priority = 0,
                    Frequency = 0
                };

                unitOfWork.Repository<Question>().Insert(entity);
                unitOfWork.SaveChanges();
                result = true;
            }

            return result;

        }

        private bool IsDuplicateQuestion(QuestionViewModel question)
        {
            bool result = false;
            //***
            //code check duplicate here
            //***

            return result;
        }

        public List<Question> GetQuestionsByCourse(int CourseId)
        {
            List<Question> Questions = unitOfWork.Repository<Question>().GetAll().ToList();
            List<Question> QuestionsByCourse = (from q in Questions
                                               where q.CourseId == CourseId
                                               select q).ToList();
            return QuestionsByCourse;
        }

        public Question GetQuestionById (int id )
        {
            Question QuestionById = u.Repository<Question>().GetById(id);
            return QuestionById;
        }

        public bool UpdateQuestion(Question question)
        {
            Question ques = u.Repository<Question>().GetById(question.Id);
            ques.QuestionContent = question.QuestionContent;
            ques.LevelId = question.LevelId;
            ques.LearningOutcomeId = question.LearningOutcomeId;
            ques.TopicId = question.TopicId;

            u.Repository<Question>().Update(ques);
            u.SaveChanges();
            return true;
        }
    }
}
