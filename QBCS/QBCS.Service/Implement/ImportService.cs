using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
                    CourseId = import.CourseId.Value,
                    NumberOfSuccess = import.TotalSuccess.HasValue ? import.TotalSuccess.Value : 0, //fix here
                    Questions = import.QuestionTemps.Select(q => new QuestionTempViewModel
                    {
                        Id = q.Id,
                        QuesitonContent = q.QuestionContent,
                        Status = (StatusEnum)q.Status,
                        ImportId = importId,
                        Code = q.Code,
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
                    Date = im.ImportedDate.Value,
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
            var rules = unitOfWork.Repository<Rule>().GetAll().Where(r => r.IsDisable == false && r.IsUse == true);
            foreach (var tempQuestion in tempQuestions)
            {
                foreach(var option1 in tempQuestion.OptionTemps)
                {
                    foreach(var option2 in tempQuestion.OptionTemps)
                    {
                        if (option1.Id == option2.Id)
                        {
                            break;
                        }
                        var trimOption1 = TrimOption(option1.OptionContent);
                        var trimOption2 = TrimOption(option2.OptionContent);
                        if (trimOption1.Equals(trimOption2))
                        {
                            tempQuestion.Status = (int)StatusEnum.Invalid;
                            break;
                        }
                    }
                    if(tempQuestion.Status == (int)StatusEnum.Invalid)
                    {
                        break;
                    }
                }
                foreach (var rule in rules)
                {
                    if (DateTime.Compare(DateTime.Now, (DateTime)rule.ActivateDate) >= 0)
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
                                if (!rule.Value.Contains("·case_sensitive·"))
                                {
                                    var varRule = rule.Value.Replace("·case_sensitive·", "");
                                    var culture = CultureInfo.GetCultureInfo("en-GB");
                                    if (culture.CompareInfo.IndexOf(rule.Value, varRule, CompareOptions.IgnoreCase) >= 0)
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                    }
                                }
                                else
                                {
                                    var varRule = rule.Value.Replace("·case_sensitive·", "");
                                    if (tempQuestion.QuestionContent.Contains(varRule))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                    }
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
                                        break;
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
                                        break;
                                    }
                                }
                                break;
                            //check option length difference
                            case 8: break;
                            //check banned words in option
                            case 9:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if (!rule.Value.Contains("·case_sensitive·"))
                                    {
                                        var varRule = rule.Value.Replace("·case_sensitive·", "");
                                        var culture = CultureInfo.GetCultureInfo("en-GB");
                                        if (culture.CompareInfo.IndexOf(option.OptionContent, varRule, CompareOptions.IgnoreCase) >= 0)
                                        {
                                            tempQuestion.Status = (int)StatusEnum.Invalid;
                                        }
                                    }
                                    else
                                    {
                                        var varRule = rule.Value.Replace("·case_sensitive·", "");
                                        if (option.OptionContent.Contains(varRule))
                                        {
                                            tempQuestion.Status = (int)StatusEnum.Invalid;
                                        }
                                    }
                                }
                                break;
                            //check min length in correct option
                            case 10:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((bool)option.IsCorrect && option.OptionContent.Length < int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        break;
                                    }
                                }
                                break;
                            //check min length in correct option
                            case 11:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((bool)option.IsCorrect && option.OptionContent.Length > int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        break;
                                    }
                                }
                                break;
                            //check banned words in correct option
                            case 12:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((bool)option.IsCorrect)
                                    {
                                        if (!rule.Value.Contains("·case_sensitive·"))
                                        {
                                            var varRule = rule.Value.Replace("·case_sensitive·", "");
                                            var culture = CultureInfo.GetCultureInfo("en-GB");
                                            if (culture.CompareInfo.IndexOf(option.OptionContent, varRule, CompareOptions.IgnoreCase) >= 0)
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                            }
                                        }
                                        else
                                        {
                                            var varRule = rule.Value.Replace("·case_sensitive·", "");
                                            if (option.OptionContent.Contains(varRule))
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                            }
                                        }
                                    }
                                }
                                break;
                            //check min length in incorrect option
                            case 13:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((!(bool)option.IsCorrect) && option.OptionContent.Length < int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        break;
                                    }
                                }
                                break;
                            //check min length in incorrect option
                            case 14:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((!(bool)option.IsCorrect) && option.OptionContent.Length > int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        break;
                                    }
                                }
                                break;
                            //check banned words in incorrect option
                            case 15:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if (!(bool)option.IsCorrect)
                                    {
                                        if (!rule.Value.Contains("·case_sensitive·"))
                                        {
                                            var varRule = rule.Value.Replace("·case_sensitive·", "");
                                            var culture = CultureInfo.GetCultureInfo("en-GB");
                                            if (culture.CompareInfo.IndexOf(option.OptionContent, varRule, CompareOptions.IgnoreCase) >= 0)
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                            }
                                        }
                                        else
                                        {

                                            var varRule = rule.Value.Replace("·case_sensitive·", "");
                                            if (option.OptionContent.Contains(varRule))
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                            }
                                        }
                                    }
                                }
                                break;
                            //check allow longest correct option
                            case 16:
                                if (!rule.Value.Contains("True"))
                                {
                                    var testOption = tempQuestion.OptionTemps.OrderByDescending(o => o.OptionContent.Length).ToList();
                                    var varOption = testOption.First();
                                    if ((bool)varOption.IsCorrect)
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                    }
                                }
                                break;
                            //check allow longest correct option
                            case 17:
                                if (!rule.Value.Contains("True"))
                                {
                                    var testOption = tempQuestion.OptionTemps.OrderBy(o => o.OptionContent.Length).ToList();
                                    var varOption = testOption.First();
                                    if ((bool)varOption.IsCorrect)
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
        private string TrimOption(string option)
        {
            string trim = option.Replace(" ", "");
            trim = trim.Replace(".", "");
            trim = trim.Replace(",", "");
            return trim;
        }
        public void UpdateQuestionTempStatus(int questionTempId, int status)
        {
            var questionTemp = unitOfWork.Repository<QuestionTemp>().GetById(questionTempId);
            if (questionTemp != null && (questionTemp.Status == (int)StatusEnum.DeleteOrSkip || questionTemp.Status == (int)StatusEnum.Delete))
            {
                questionTemp.Status = status;
                unitOfWork.Repository<QuestionTemp>().Update(questionTemp);
                unitOfWork.SaveChanges();
            }
        }
    }
}
