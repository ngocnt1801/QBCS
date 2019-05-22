using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;

namespace QBCS.Service.Implement
{
    public class QuestionService : IQuestionService
    {
        private IUnitOfWork u;

        public QuestionService()
        {
            u = new UnitOfWork();
        }
        public List<Question> GetQuestionsByCourse(int CourseId)
        {
            List<Question> Questions = u.Repository<Question>().GetAll().ToList();
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
