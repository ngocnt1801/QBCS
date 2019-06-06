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

                learningOutcomeViewModels.Add(learningOutcomeViewModel);
            }

            return learningOutcomeViewModels;
        }
        public List<LearningOutcomeViewModel> GetAllLearningOutcome()
        {
            List<LearningOutcome> learningOutcomes = u.Repository<LearningOutcome>().GetAll().Where(lo => lo.IsDisable == false).ToList();
            List<LearningOutcomeViewModel> result = new List<LearningOutcomeViewModel>();
            foreach(var learningOutcome in learningOutcomes)
            {
                LearningOutcomeViewModel model = new LearningOutcomeViewModel()
                {
                    Id = learningOutcome.Id,
                    Code = learningOutcome.Code,
                    Name = learningOutcome.Name,
                    CourseId = (int)learningOutcome.CourseId
                };
                result.Add(model);
            }
            return result;
        }
        public bool UpdateDisable(int id)
        {
            try
            {
                var learningOutcome = u.Repository<LearningOutcome>().GetById(id);
                learningOutcome.IsDisable = true;
                u.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddLearningOutcome(LearningOutcomeViewModel model)
        {
            try
            {
                var learningOutcome = new LearningOutcome()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CourseId = model.CourseId,
                    IsDisable = false
                };
                u.Repository<LearningOutcome>().Insert(learningOutcome);
                u.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateLearningOutcome(LearningOutcomeViewModel model)
        {
            try
            {
                var learningOutcome = u.Repository<LearningOutcome>().GetById(model.Id);
                learningOutcome.Code = model.Code;
                learningOutcome.Name = model.Name;
                learningOutcome.CourseId = model.CourseId;
                u.Repository<LearningOutcome>().Update(learningOutcome);
                u.SaveChanges();
                return true;
            } catch
            {
                return false;
            }
            
        }
        public LearningOutcomeViewModel GetLearningOutcomeById(int id)
        {
            var learningOutcome = u.Repository<LearningOutcome>().GetById(id);
            var result = new LearningOutcomeViewModel()
            {
                Id = learningOutcome.Id,
                Code = learningOutcome.Code,
                Name = learningOutcome.Name,
                CourseId = (int)learningOutcome.CourseId
            };
            return result;
        }
    }
}
