using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace QBCS.Service.Interface
{
    public class SyllabusService : ISyllabusService
    {
        private IUnitOfWork unitOfWork;

        public SyllabusService()
        {
            unitOfWork = new UnitOfWork();
        }

        public void AddSyllabusPartial(SyllabusPartialViewModel model)
        {
            unitOfWork.Repository<SyllabusPartial>().Insert(new SyllabusPartial
            {
                Name = model.Name,
                AmountQuestion = model.Amount,
                CourseId = model.CourseId
            });
            unitOfWork.SaveChanges();
        }

        public void ChangeSyllabusPartial(int locId, int? syllabusParitalId)
        {
            var loc = unitOfWork.Repository<LearningOutcome>().GetById(locId);
            if (loc != null)
            {
                loc.SyllabusId = syllabusParitalId;
                unitOfWork.Repository<LearningOutcome>().Update(loc);
                unitOfWork.SaveChanges();
            }
        }

        public void DeleteSyllabusPartial(int syllabusId)
        {
            var syllabusPartial = unitOfWork.Repository<SyllabusPartial>().GetById(syllabusId);
            foreach (var loc in syllabusPartial.LearningOutcomes.ToList())
            {
                loc.SyllabusId = null;
                unitOfWork.Repository<LearningOutcome>().Update(loc);
            }
            unitOfWork.Repository<SyllabusPartial>().Delete(syllabusPartial);
            unitOfWork.SaveChanges();
        }

        public List<LearningOutcomeViewModel> GetLearningOutcomes(int? syllabusPartialId)
        {
            return unitOfWork.Repository<LearningOutcome>().GetAll()
                    .Where(l => l.SyllabusId == syllabusPartialId)
                    .Select(l => new LearningOutcomeViewModel
                    {
                        Id = l.Id,
                        Name = l.Name,
                        SyllabusId = l.SyllabusId
                    })
                    .ToList();
        }

        public List<SyllabusPartialViewModel> GetSyllabusPartials(int courseId)
        {
            return unitOfWork.Repository<SyllabusPartial>()
                .GetAll()
                .Where(s => s.CourseId == courseId)
                .Select(s => new SyllabusPartialViewModel
                {
                    Id = s.Id,
                    Amount = s.AmountQuestion.Value,
                    Name = s.Name,
                    CourseId= s.CourseId.Value
                })
                .ToList();
        }

        public void UpdateSyllabusPartial(SyllabusPartialViewModel model)
        {
            var entity = unitOfWork.Repository<SyllabusPartial>().GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.AmountQuestion = model.Amount;
                unitOfWork.Repository<SyllabusPartial>().Update(entity);
                unitOfWork.SaveChanges();
            }
        }
    }
}
