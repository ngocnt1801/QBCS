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
        public List<LearningOutcomeViewModel> GetLearningOutcomeByCourseId(int CourseId)
        {
            IQueryable<LearningOutcome> LearningOutcomes = u.Repository<LearningOutcome>().GetAll();

            List<LearningOutcome> LearningOutcomeByCourse = LearningOutcomes.Where(lo => lo.CourseId == CourseId).ToList();

            List<LearningOutcomeViewModel> learningOutcomeViewModels = new List<LearningOutcomeViewModel>();

            foreach (var learningOutcome in LearningOutcomeByCourse)
            {
                LearningOutcomeViewModel learningOutcomeViewModel = new LearningOutcomeViewModel()
                {
                    Id = learningOutcome.Id,
                    Code = learningOutcome.Code,
                    CourseId = (int) learningOutcome.CourseId,
                    IsDisable = (bool) learningOutcome.IsDisable,
                    Name = learningOutcome.Name
                };
                learningOutcomeViewModel.UpdateIdValue();
                learningOutcomeViewModels.Add(learningOutcomeViewModel);
            }

            return learningOutcomeViewModels;
        }
    }
}
