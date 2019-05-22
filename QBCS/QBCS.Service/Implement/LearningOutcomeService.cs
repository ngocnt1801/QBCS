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
    public class LearningOutcomeService : ILearningOutcomeService
    {

        private IUnitOfWork u;

        public LearningOutcomeService()
        {
            u = new UnitOfWork();
        }
        public List<LearningOutcome> GetLearningOutcomeByCourseId(int? CourseId)
        {
            List<LearningOutcome> LearningOutcomes = u.Repository<LearningOutcome>().GetAll().ToList();

            List<LearningOutcome> LearningOutcomeByCourse = LearningOutcomes.Where(lo => lo.CourseId == CourseId).ToList();

            return LearningOutcomeByCourse;
        }
    }
}
