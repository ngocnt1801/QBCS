using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
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

        public void Cancel(int importId)
        {
            var import = unitOfWork.Repository<Import>().GetById(importId);
            var listQuestion = import.QuestionTemps.ToList();
            if (import != null)
            {
                foreach (var question in listQuestion)
                {
                    unitOfWork.Repository<QuestionTemp>().Delete(question);
                }

                import.Status = (int)StatusEnum.Canceled;
                unitOfWork.Repository<Import>().Update(import);
                unitOfWork.SaveChanges();
            }
        }

        public ImportResultViewModel GetImportResult(int importId)
        {
            var import = unitOfWork.Repository<Import>().GetAll().Where(i => i.Id == importId).FirstOrDefault();
            if (import != null)
            {
                if (import.Status != (int)StatusEnum.Done)
                {
                    import.Status = (int)StatusEnum.Editing;
                }
                import.Seen = true;
                unitOfWork.Repository<Import>().Update(import);
                unitOfWork.SaveChanges();

                return new ImportResultViewModel
                {
                    Id = import.Id,
                    Status = import.Status.Value,
                    NumberOfSuccess = import.TotalSuccess.HasValue ? import.TotalSuccess.Value : 0, //fix here
                    Questions = import.QuestionTemps.Select(q => new QuestionTempViewModel
                    {
                        Id = q.Id,
                        QuesitonContent = q.QuestionContent,
                        Status = (StatusEnum)q.Status,
                        ImportId = importId,
                        Code = q.Code,
                        Image = q.Image,
                        DuplicatedQuestion = q.DuplicatedId.HasValue ? new QuestionViewModel
                        {
                            Id = q.DuplicatedWithBank.Id,
                            CourseName = "Bank: " + q.DuplicatedWithBank.Course.Name,
                            Code = q.DuplicatedWithBank.QuestionCode,
                            QuestionContent = q.DuplicatedWithBank.QuestionContent,
                            Options = q.DuplicatedWithBank.Options.Select(o => new OptionViewModel
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                            }).ToList()
                        } : (q.DuplicateInImportId.HasValue ? new QuestionViewModel {
                            Id = q.DuplicatedWithImport.Id,
                            Code = q.DuplicatedWithImport.Code,
                            CourseName = "Import File",
                            QuestionContent = q.DuplicatedWithImport.QuestionContent,
                            Options = q.DuplicatedWithImport.OptionTemps.Select(o => new OptionViewModel {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                            }).ToList()
                        } : null),
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

        public List<ImportViewModel> GetListImport(int userId)
        {
            return unitOfWork.Repository<Import>().GetAll()
                .Where(im => im.UserId == userId)
                .Select(im => new ImportViewModel
                {
                    Id = im.Id,
                    Date = im.InsertedToBankDate.Value,
                    Status = (StatusEnum)im.Status.Value,
                    TotalQuestion = im.TotalQuestion.HasValue ? im.TotalQuestion.Value : 0,
                    TotalSuccess = im.TotalSuccess.HasValue ? im.TotalSuccess.Value : 0
                })
                .OrderByDescending(im => im.Date)
                .ToList();
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
                    DuplicatedQuestion = questionTemp.DuplicatedWithBank != null ? (new QuestionViewModel
                    {
                        Id = questionTemp.DuplicatedWithBank.Id,
                        CourseName = questionTemp.DuplicatedWithBank.Course.Name,
                        Code = questionTemp.DuplicatedWithBank.QuestionCode,
                        QuestionContent = questionTemp.DuplicatedWithBank.QuestionContent,
                        Options = questionTemp.DuplicatedWithBank.Options.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                        }).ToList()
                    }) : (new QuestionViewModel
                    {
                        Id = questionTemp.DuplicatedWithImport.Id,
                        CourseName = "Import file",
                        Code = questionTemp.DuplicatedWithImport.Code,
                        QuestionContent = questionTemp.DuplicatedWithImport.QuestionContent,
                        Options = questionTemp.DuplicatedWithImport.OptionTemps.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                        }).ToList()
                    }),
                    Options = questionTemp.OptionTemps.Select(o => new OptionViewModel
                    {
                        Id = o.Id,
                        OptionContent = o.OptionContent,
                        IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                    }).ToList()
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
                foreach (var option in entity.OptionTemps)
                {
                    var updatedOption = question.Options.Where(o => o.Id == option.Id).FirstOrDefault();
                    if (updatedOption != null)
                    {
                        option.IsCorrect = updatedOption.IsCorrect;
                        option.OptionContent = updatedOption.OptionContent;
                    }
                }

                unitOfWork.Repository<QuestionTemp>().Update(entity);
                unitOfWork.SaveChanges();
            }
        }

        public List<QuestionTemp> CheckRule(List<QuestionTemp> tempQuestions)
        {
            var rules = unitOfWork.Repository<Rule>().GetAll().Where(r => r.IsDisable == false);
            foreach(var tempQuestion in tempQuestions)
            {
                foreach (var rule in rules)
                {
                    if(DateTime.Compare(DateTime.Now, (DateTime)rule.ActivateDate) >= 0)
                    {
                        switch (rule.KeyId)
                        {
                            //check min question length
                            case 1:
                                if (tempQuestion.QuestionContent.Length < int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                }
                                break;
                            //check max question length
                            case 2:
                                if (tempQuestion.QuestionContent.Length > int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                }
                                break;
                            //check banned words in question
                            case 3:
                                if (tempQuestion.QuestionContent.Contains(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                }
                                break;
                            //check min options count in question
                            case 4:
                                if (tempQuestion.OptionTemps.Count < int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                }
                                break;
                            //check max option count in question
                            case 5:
                                if (tempQuestion.OptionTemps.Count > int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                }
                                break;
                            //check min option length
                            case 6:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if (option.OptionContent.Length < int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                    }
                                }
                                break;
                            //check max option length
                            case 7:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if (option.OptionContent.Length > int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                    }
                                }
                                break;
                            //check option length difference
                            case 8: break;
                            //check banned words in option
                            case 9:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if (option.OptionContent.Contains(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                    }
                                }
                                break;
                        }
                        if (tempQuestion.Status == (int)StatusEnum.Invalid)
                        {
                            break;
                        }
                    }   
                }
            }
                return tempQuestions;
        }

        public void UpdateQuestionTempStatus(int questionTempId, int status)
        {
            var questionTemp = unitOfWork.Repository<QuestionTemp>().GetById(questionTempId);
            if (questionTemp != null && questionTemp.Status == (int)StatusEnum.DeleteOrSkip)
            {
                questionTemp.Status = status;
                unitOfWork.Repository<QuestionTemp>().Update(questionTemp);
                unitOfWork.SaveChanges();
            }
        }
    }
}
