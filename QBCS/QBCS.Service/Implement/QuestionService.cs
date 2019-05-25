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

        public List<Question> GetQuestionsByContent(string questionContent)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll().Where(q =>  q.QuestionContent.Contains(questionContent));
            List<Question> result = questions.ToList();
            return result;
        }

        public List<Question> GetQuestionSearchBar(string searchInput)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll().Where(q => 
                                                                                            q.QuestionContent.Contains(searchInput) || 
                                                                                            q.Course.Name.Contains(searchInput))
                                                                                            .Take(5);
            List<Question> result = questions.ToList();
            return result;
        }
    }
}
