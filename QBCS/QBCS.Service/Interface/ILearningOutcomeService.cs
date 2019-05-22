using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ILearningOutcomeService
    {
        List<LearningOutcome> GetLearningOutcomeByCourseId(int? CourseId);
    }
}
