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
        ICategoryService categoryService;
        public PartOfExamService()
        {
            unitOfWork = new UnitOfWork();
            levelService = new LevelService();
            categoryService = new CategoryService();
        }
        public List<PartOfExamViewModel> GetPartOfExamByExamId(int examinationId)
        {
            IQueryable<PartOfExamination> partOfExams = unitOfWork.Repository<PartOfExamination>().GetAll();
            List<PartOfExamination> partOfExamsByExamId = partOfExams.Where(p => p.ExaminationId == examinationId).ToList();
            List<PartOfExamViewModel> result = new List<PartOfExamViewModel>();            
            foreach(PartOfExamination part in partOfExamsByExamId)
            {
                List<QuestionInExamViewModel> questions = part.QuestionInExams.Select(c => new QuestionInExamViewModel
                {
                    Id = (int)c.Id,
                    QuestionContent = c.QuestionContent,
                    Level = levelService.GetLevelById(c.LevelId.HasValue ? (int)c.LevelId : 0),
                    LevelId = c.LevelId.HasValue ? (int)c.LevelId : 0,
                    CategoryId = c.CategoryId.HasValue ? (int)c.CategoryId : 0,                   
                    QuestionCode = c.QuestionCode,
                    Image = c.Image,
                    Options = c.OptionInExams.Select(d => new OptionViewModel
                    {
                        Id = d.Id,
                        OptionContent = d.OptionContent,
                        IsCorrect = (bool)d.IsCorrect,
                        Image = d.Image
                    }).ToList()
                }).ToList();
                
                PartOfExamViewModel partViewModel = new PartOfExamViewModel()
                {
                    ExaminationId = part.ExaminationId.HasValue ? (int)part.ExaminationId : 0,
                    Question = questions
                };
                if (part.LearningOutcome != null)
                {
                    partViewModel.LearningOutcome = new LearningOutcomeViewModel()
                    {

                        Id = part.LearningOutcome.Id,
                        Code = part.LearningOutcome.Code != null ? part.LearningOutcome.Code : "",
                        CourseId = part.LearningOutcome.CourseId.HasValue ? part.LearningOutcome.CourseId.Value : 0,
                        IsDisable = part.LearningOutcome.IsDisable.HasValue && part.LearningOutcome.IsDisable.Value,
                        Name = part.LearningOutcome.Name
                    };
                }
                result.Add(partViewModel);
            };
            return result;
        }
    }
}
