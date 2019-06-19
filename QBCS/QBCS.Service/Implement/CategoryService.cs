using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace QBCS.Service.Implement
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork unitOfWork;
        public CategoryService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<CategoryViewModel> GetCategoriesByCourseId(int courseId)
        {
            List<CategoryViewModel> categoriesByCourseId = unitOfWork.Repository<Category>().GetAll().Where(c => c.CourseId == courseId).Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return categoriesByCourseId;
        }

        public CategoryViewModel GetCategoryById(int categoryId)
        {
            Category category = unitOfWork.Repository<Category>().GetById(categoryId);
            if (category != null)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name
                };
                return categoryViewModel;
            }
            else
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    Id = 0,
                    Name = "[No Category]"
                };
                return categoryViewModel;
            }
        }

        public List<CategoryViewModel> GetListCategories(int courseId)
        {
            var categories = unitOfWork.Repository<Category>().GetAll()
                                                                .Where(c => c.CourseId == courseId)
                                                                .Select(c => new CategoryViewModel
                                                                {
                                                                    Id = c.Id,
                                                                    Name = c.Name,
                                                                })
                                                                .ToList();

            foreach (var category in categories)
            {
                var categoryQuestions = unitOfWork.Repository<Question>().GetAll().Where(q => q.CategoryId == category.Id && !(q.IsDisable.HasValue && q.IsDisable.Value));
                #region has category
                category.QuestionCount = categoryQuestions.Count();

                category.LearningOutcomes = categoryQuestions.Where(q => q.LearningOutcomeId != null || q.TopicId != null).Select(q => new LearningOutcomeViewModel
                {
                    Id = q.LearningOutcomeId.HasValue ? q.LearningOutcomeId.Value : 0,
                    Name = q.LearningOutcome.Name,
                    IsLearningOutcome = true
                })
                                                .Distinct()
                                                .ToList();

                category.LearningOutcomes.AddRange(categoryQuestions.Where(q => q.LearningOutcomeId != null || q.TopicId != null).Select(q => new LearningOutcomeViewModel
                {
                    Id = q.TopicId.HasValue ? q.TopicId.Value : 0,
                    Name = q.Topic.Name,
                    IsLearningOutcome = false
                })
                                                        .Distinct()
                                                        .ToList());


                foreach (var lo in category.LearningOutcomes)
                {
                    var loQuestion = unitOfWork.Repository<Question>().GetAll().Where(q => q.CategoryId == category.Id
                                                                                            && (lo.IsLearningOutcome ? q.LearningOutcomeId == lo.Id : q.TopicId == lo.Id)
                                                                                            && !(q.IsDisable.HasValue && q.IsDisable.Value));

                    lo.QuestionCount = loQuestion.Count();
                    lo.Levels = loQuestion.Select(q => new LevelViewModel
                    {
                        Id = q.LevelId.HasValue ? q.LevelId.Value : 0,
                        Name = q.Level.Name,
                    })
                                            .Distinct()
                                            .OrderBy(l => l.Id)
                                            .ToList();

                    foreach (var lv in lo.Levels)
                    {
                        lv.QuestionCount = unitOfWork.Repository<Question>().GetAll().Where(q => q.CategoryId == category.Id
                                                                                                && (lo.IsLearningOutcome ? q.LearningOutcomeId == lo.Id : q.TopicId == lo.Id)
                                                                                                && q.LevelId == lv.Id
                                                                                                && !(q.IsDisable.HasValue && q.IsDisable.Value))
                                                                                    .Count();
                    }

                }
                #endregion

                
            }

            #region no category
            var noCategoryQuestions = unitOfWork.Repository<Question>().GetAll().Where(q => q.CourseId == courseId && q.CategoryId == null && !(q.IsDisable.HasValue && q.IsDisable.Value));
            if (noCategoryQuestions.Any())
            {
                categories.Add(new CategoryViewModel
                {
                    QuestionCount = noCategoryQuestions.Count(),
                    Name = "[No Category]",
                    Id = 0
                });
            }
            #endregion
            return categories;
        }
    }
}
