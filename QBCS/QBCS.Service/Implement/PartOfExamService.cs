using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Service.ViewModel;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Entity;

namespace QBCS.Service.Implement
{
    public class PartOfExamService : IPartOfExamService
    {
        private IUnitOfWork unitOfWork;
        private ILevelService levelService;
        public PartOfExamService()
        {
            unitOfWork = new UnitOfWork();
            levelService = new LevelService();
        }
        public List<PartOfExamViewModel> GetPartOfExamByExamId(int examinationId)
        {
            IQueryable<PartOfExamination> partOfExams = unitOfWork.Repository<PartOfExamination>().GetAll();
            List<PartOfExamination> partOfExamsByExamId = partOfExams.Where(p => p.ExaminationId == examinationId).ToList();
            List<PartOfExamViewModel> result = new List<PartOfExamViewModel>();
            foreach(PartOfExamination part in partOfExamsByExamId)
            {
                List<QuestionViewModel> questions = part.QuestionInExams.Select(c => new QuestionViewModel
                {

                    Id = (int)c.Id,
                    QuestionContent = c.QuestionContent,
                    Level = levelService.GetLevelById((int)c.LevelId),
                    Options = c.OptionInExams.Select(d => new OptionViewModel
                    {
                        Id = d.Id,
                        OptionContent = d.OptionContent,
                        IsCorrect = (bool)d.IsCorrect
                    }).ToList()
                }).ToList();
                
                PartOfExamViewModel partViewModel = new PartOfExamViewModel()
                {
                    ExaminationId = (int)part.ExaminationId,
                    Question = questions
                };
                if (part.LearningOutcome != null)
                {
                    partViewModel.LearningOutcome = new LearningOutcomeViewModel()
                    {

                        Id = part.LearningOutcome.Id,
                        Code = part.LearningOutcome.Code,
                        CourseId = (int)part.LearningOutcome.CourseId,
                        IsDisable = (bool)part.LearningOutcome.IsDisable,
                        Name = part.LearningOutcome.Name
                    };
                } else
                {
                    partViewModel.Topic = new TopicViewModel()
                    {
                        Id = part.Topic.Id,
                        Name = part.Topic.Name,
                        Code = part.Topic.Code,
                        CourseId = (int)part.Topic.CourseId,
                        IsDisable = (bool)part.Topic.IsDisable
                    };
                }
                result.Add(partViewModel);
            };
            return result;
        }
    }
}
