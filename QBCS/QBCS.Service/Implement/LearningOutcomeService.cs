using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Service.ViewModel;

namespace QBCS.Service.Implement
{
    public class LearningOutcomeService : ILearningOutcomeService
    {

        private IUnitOfWork u;

        public LearningOutcomeService()
        {
            u = new UnitOfWork();
        }
        public List<LearningOutcomeViewModel> GetLearningOutcomeByCourseId(int? CourseId)
        {
            List<LearningOutcome> LearningOutcomes = u.Repository<LearningOutcome>().GetAll().ToList();

            List<LearningOutcome> LearningOutcomeByCourse = LearningOutcomes.Where(lo => lo.CourseId == CourseId).ToList();

            List<LearningOutcomeViewModel> LearningOutcomeViewModels = new List<LearningOutcomeViewModel>();

            foreach (var lo in LearningOutcomeByCourse)
            {
                LearningOutcomeViewModel lovm = new LearningOutcomeViewModel()
                {
                    Id = lo.Id,
                    Code = lo.Code,
                    CourseId = (int) lo.CourseId,
                    IsDisable = (bool) lo.IsDisable,
                    Name = lo.Name
                };

                LearningOutcomeViewModels.Add(lovm);
            }

            return LearningOutcomeViewModels;
        }
    }
}
