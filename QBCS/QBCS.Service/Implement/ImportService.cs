using DuplicateQuestion.Entity;
using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class ImportService : IImportService
    {
        private IUnitOfWork unitOfWork;

        public ImportService()
        {
            unitOfWork = new UnitOfWork();
        }

        public ImportResultViewModel GetImportResult(int importId)
        {
            var import = unitOfWork.Repository<Import>().GetAll().Where(i => i.Id == importId).FirstOrDefault();
            if (import != null)
            {
                import.Status = (int)StatusEnum.Fixing;
                import.Seen = true;
                unitOfWork.Repository<Import>().Update(import);
                unitOfWork.SaveChanges();

                return new ImportResultViewModel
                {
                    Id = import.Id,
                    Questions = import.QuestionTemps.Select(q => new QuestionTempViewModel
                    {
                        Id = q.Id,
                        QuesitonContent = q.QuestionContent,
                        Status = (StatusEnum)q.Status,
                        ImportId = importId,
                        DuplicatedQuestion = q.DuplicatedId.HasValue ? new QuestionViewModel
                        {
                            Id = q.Question.Id,
                            CourseName = q.Question.Course.Name,
                            Code = q.Question.QuestionCode,
                            QuestionContent = q.Question.QuestionContent,
                            Options = q.Question.Options.Select(o => new OptionViewModel
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                            }).ToList()
                        } : null,
                        Options = q.OptionTemps.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                        }).ToList()
                    }).OrderBy(q => q.Status).ToList() 
                };
            }

            return null;
        }

        public QuestionTempViewModel GetQuestionTemp(int questionTempId)
        {
            var questionTemp = unitOfWork.Repository<QuestionTemp>().GetById(questionTempId);
            if (questionTemp != null)
            {
                return new QuestionTempViewModel
                {
                    Id = questionTemp.Id,
                    QuesitonContent = questionTemp.QuestionContent,
                    Status = (StatusEnum)questionTemp.Status,
                    ImportId = questionTemp.ImportId.Value,
                    DuplicatedQuestion = new QuestionViewModel
                    {
                        Id = questionTemp.Question.Id,
                        CourseName = questionTemp.Question.Course.Name,
                        Code = questionTemp.Question.QuestionCode,
                        QuestionContent = questionTemp.Question.QuestionContent,
                        Options = questionTemp.Question.Options.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                        }).ToList()
                    }
                };
            }

            return null;
        }

        public async Task ImportToBank(int importId)
        {
            using (var context = new QBCSContext())
            {
                string command = "EXEC CheckDuplicateImport @importId= @id";
                await context.Database.ExecuteSqlCommandAsync(command, new SqlParameter("@id", importId));
            }
        }

        public void UpdateQuestionTemp(QuestionTempViewModel question)
        {
            var entity = unitOfWork.Repository<QuestionTemp>().GetById(question.Id);
            if (entity != null && entity.Status == (int)StatusEnum.Editable)
            {
                entity.QuestionContent = question.QuesitonContent;
                entity.Status = (int)StatusEnum.NotCheck;

                unitOfWork.Repository<QuestionTemp>().Update(entity);
                unitOfWork.SaveChanges();
            }
        }
    }
}
