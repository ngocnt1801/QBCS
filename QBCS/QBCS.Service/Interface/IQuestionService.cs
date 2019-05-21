using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IQuestionService
    {
        List<Question> GetQuestionsByCourse(int CourseId);
    }
}
