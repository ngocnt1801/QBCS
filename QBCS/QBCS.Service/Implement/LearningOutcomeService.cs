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

        private IUnitOfWork unitOfWork;

        public LearningOutcomeService()
        {
            unitOfWork = new UnitOfWork();
        }

        public int GetCourseIdByLearningOutcomeId(int learningOutcomeId)
        {
            LearningOutcome learningOutcome = unitOfWork.Repository<LearningOutcome>().GetById(learningOutcomeId);
            return (int)learningOutcome.CourseId;
        }

        public LearningOutcomeViewModel GetLearingOutcomeById(int learningOutcomeId)
        {
            LearningOutcome learningOutcome = unitOfWork.Repository<LearningOutcome>().GetById(learningOutcomeId);
            LearningOutcomeViewModel learningOutcomeViewModel = new LearningOutcomeViewModel()
            {
                Id = learningOutcome.Id,
                //Code = learningOutcome.Code != null ? learningOutcome.Code.ToString() : "", //bo field nay dc khong
                CourseId = (int)learningOutcome.CourseId != null ? learningOutcome.CourseId.Value : 0,
                //IsDisable = (bool)learningOutcome.IsDisable != null ? learningOutcome.IsDisable.Value : true, //bo field nay dc khong
                Name = learningOutcome.Name
            };
            return learningOutcomeViewModel;
        }

        public List<LearningOutcomeViewModel> GetLearningOutcomeByCourseId(int CourseId)
        {
            IQueryable<LearningOutcome> LearningOutcomes = unitOfWork.Repository<LearningOutcome>().GetAll();

            List<LearningOutcome> LearningOutcomeByCourse = LearningOutcomes.Where(lo => lo.CourseId == CourseId).ToList();

            List<LearningOutcomeViewModel> learningOutcomeViewModels = new List<LearningOutcomeViewModel>();

            foreach (var learningOutcome in LearningOutcomeByCourse)
            {
                LearningOutcomeViewModel learningOutcomeViewModel = new LearningOutcomeViewModel()
                {
                    Id = learningOutcome.Id,
                    Code = learningOutcome.Code != null ? learningOutcome.Code : "", 
                    CourseId = learningOutcome.CourseId.Value,
                    IsDisable =  learningOutcome.IsDisable.HasValue && learningOutcome.IsDisable.Value,
                    Name = learningOutcome.Name
                };
                learningOutcomeViewModel.UpdateIdValue();
                learningOutcomeViewModels.Add(learningOutcomeViewModel);
            }

            return learningOutcomeViewModels;
        }
        public List<LearningOutcomeViewModel> GetAllLearningOutcome()
        {
            List<LearningOutcome> learningOutcomes = unitOfWork.Repository<LearningOutcome>().GetAll().Where(lo => lo.IsDisable == false).ToList();
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
        public int UpdateDisable(int id)
        {
            try
            {
                var learningOutcome = unitOfWork.Repository<LearningOutcome>().GetById(id);
                learningOutcome.IsDisable = true;
                unitOfWork.SaveChanges();
                return (int)learningOutcome.CourseId;
            }
            catch
            {
                return 0;
            }
        }
        public int AddLearningOutcome(LearningOutcomeViewModel model)
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
                var lo = unitOfWork.Repository<LearningOutcome>().InsertAndReturn(learningOutcome);
                unitOfWork.SaveChanges();

                return (int)lo.CourseId;
            }
            catch
            {
                return 0;
            }
        }
        public int UpdateLearningOutcome(LearningOutcomeViewModel model)
        {
            try
            {
                var learningOutcome = unitOfWork.Repository<LearningOutcome>().GetById(model.Id);
                learningOutcome.Code = model.Code;
                learningOutcome.Name = model.Name;
                //learningOutcome.CourseId = model.CourseId;
                unitOfWork.Repository<LearningOutcome>().Update(learningOutcome);
                unitOfWork.SaveChanges();
                return model.CourseId;
            } catch
            {
                return 0;
            }
            
        }
        public LearningOutcomeViewModel GetLearningOutcomeById(int id)
        {
            var learningOutcome = unitOfWork.Repository<LearningOutcome>().GetById(id);
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
