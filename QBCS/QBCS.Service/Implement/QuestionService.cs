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
    }
}
