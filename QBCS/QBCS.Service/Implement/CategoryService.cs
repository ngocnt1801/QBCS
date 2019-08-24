using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System;
using QBCS.Service.Utilities;
using System.Data.SqlClient;

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
            var categories = unitOfWork.Repository<Question>().GetAll()
                                                                .Where(q => q.CourseId == courseId)
                                                                .Select(q => new CategoryViewModel
                                                                {
                                                                    Id = q.CategoryId,
                                                                    Name = q.CategoryId.HasValue ? q.Category.Name : "[None of category]"
                                                                })
                                                                .Distinct()
                                                                .OrderBy(c => c.Name)
                                                                .ToList();

            foreach (var category in categories)
            {
                #region get learning outcome
                var listString = new List<string>();
                var categoryLOs = new List<LearningOutcomeViewModel>();
                var categoryQuestions = unitOfWork.Repository<Question>().GetAll().Where(q => q.CourseId == courseId && q.CategoryId == category.Id && !(q.IsDisable.HasValue && q.IsDisable.Value));

                category.QuestionCount = categoryQuestions.Count();

                category.LearningOutcomes = categoryQuestions.Select(q => new LearningOutcomeViewModel
                {
                    Id = q.LearningOutcomeId,
                    Name = q.LearningOutcomeId.HasValue ? q.LearningOutcome.Name : "[None of LOC]",
                })
                .Distinct()
                .OrderBy(lo => lo.Name)
                .ToList();
                foreach (var lo in category.LearningOutcomes)
                {
                    if (!listString.Contains(lo.Name))
                    {
                        listString.Add(lo.Name);
                    }   
                }
                using(NaturalSort comparer = new NaturalSort())
                {
                    listString.Sort(comparer);
                }
                foreach(string str in listString)
                {
                    var listLO = category.LearningOutcomes.Where(lo => lo.Name == str).ToList().OrderBy(lo => lo.Name);
                    foreach(var lo in listLO)
                    {
                        categoryLOs.Add(lo);
                    }
                }
                category.LearningOutcomes = categoryLOs;
                #endregion
                foreach (var lo in category.LearningOutcomes)
                {
                    #region get level
                    var loQuestion = unitOfWork.Repository<Question>().GetAll().Where(q => q.CourseId == courseId
                                                                                            && q.CategoryId == category.Id
                                                                                            && q.LearningOutcomeId == lo.Id
                                                                                            && !(q.IsDisable.HasValue && q.IsDisable.Value));

                    lo.QuestionCount = loQuestion.Count();

                    lo.Levels = loQuestion.Select(q => new LevelViewModel
                    {
                        Id = q.LevelId,
                        Name = q.LevelId.HasValue ? q.Level.Name : "[None of level]",
                    })
                    .Distinct()
                    .OrderBy(l => l.Id)
                    .ToList();

                    foreach (var lv in lo.Levels)
                    {
                        lv.QuestionCount = unitOfWork.Repository<Question>().GetAll().Where(q => q.CourseId == courseId
                                                                                                && q.CategoryId == category.Id
                                                                                                && q.LearningOutcomeId == lo.Id
                                                                                                && q.LevelId == lv.Id
                                                                                                && !(q.IsDisable.HasValue && q.IsDisable.Value))
                                                                                    .Count();
                    }
                    #endregion
                }
            }
            return categories;
        }
        public List<CategoryViewModel> GetAllCategories()
        {
            var categories = unitOfWork.Repository<Question>().GetAll()
                                                                .Select(q => new CategoryViewModel
                                                                {
                                                                    Id = q.CategoryId,
                                                                    Name = q.CategoryId.HasValue ? q.Category.Name : "[None of category]"
                                                                })
                                                                .Distinct()
                                                                .ToList();

            foreach (var category in categories)
            {
                var categoryQuestions = unitOfWork.Repository<Question>().GetAll().Where(q => q.CategoryId == category.Id && !(q.IsDisable.HasValue && q.IsDisable.Value));
                #region has category
                category.QuestionCount = categoryQuestions.Count();

                category.LearningOutcomes = categoryQuestions.Select(q => new LearningOutcomeViewModel
                {
                    Id = q.LearningOutcomeId,
                    Name = q.LearningOutcomeId.HasValue ? q.LearningOutcome.Name : "[None of LOC]",
                })
                .Distinct()
                .ToList();

                foreach (var lo in category.LearningOutcomes)
                {
                    var loQuestion = unitOfWork.Repository<Question>().GetAll().Where(q => q.CategoryId == category.Id
                                                                                            && q.LearningOutcomeId == lo.Id
                                                                                            && !(q.IsDisable.HasValue && q.IsDisable.Value));

                    lo.QuestionCount = loQuestion.Count();

                    lo.Levels = loQuestion.Select(q => new LevelViewModel
                    {
                        Id = q.LevelId,
                        Name = q.LevelId.HasValue ? q.Level.Name : "[None of level]",
                    })
                    .Distinct()
                    .OrderBy(l => l.Id)
                    .ToList();

                    foreach (var lv in lo.Levels)
                    {
                        lv.QuestionCount = unitOfWork.Repository<Question>().GetAll().Where(q => q.CategoryId == category.Id
                                                                                                && q.LearningOutcomeId == lo.Id
                                                                                                && q.LevelId == lv.Id
                                                                                                && !(q.IsDisable.HasValue && q.IsDisable.Value))
                                                                                    .Count();
                    }
                }
                #endregion
            }
            return categories;
        }

        public void AddCategory(CategoryViewModel model)
        {
            unitOfWork.Repository<Category>().Insert(new Category
            {
                Name = model.Name,
                CourseId = model.CourseId
            });
            unitOfWork.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var entity = unitOfWork.Repository<Category>().GetById(categoryId);
            if (entity != null)
            {
                unitOfWork.Repository<Category>().Delete(entity);
                unitOfWork.SaveChanges();
            }
        }

        public void UpdateCategory(CategoryViewModel model)
        {
            var entity = unitOfWork.Repository<Category>().GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                unitOfWork.Repository<Category>().Update(entity);
                unitOfWork.SaveChanges();
            }
        }

        public void DisableCategory(int categoryId)
        {
            string query = "Update Question Set IsDisable=1 Where CategoryId = @cateogry";
            unitOfWork.GetContext().Database.ExecuteSqlCommandAsync(query, new SqlParameter("@cateogry", categoryId));
        }
    }
}
