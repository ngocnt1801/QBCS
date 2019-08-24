using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class ImportService : IImportService
    {
        private IUnitOfWork unitOfWork;

        public ImportService()
        {
            unitOfWork = new UnitOfWork();//comment here
        }

        public void Cancel(int importId)
        {
            var import = unitOfWork.Repository<Import>().GetById(importId);
            if (import != null)
            {

                var listImage = import.QuestionTemps.SelectMany(q => q.OptionTemps.SelectMany(o => o.Images)).Distinct().ToList();

                foreach (var image in listImage)
                {
                    unitOfWork.Repository<Image>().Delete(image);
                }
                var listQuestion = import.QuestionTemps.OrderByDescending(q => q.Id).ToList();
                foreach (var question in listQuestion)
                {
                    unitOfWork.Repository<QuestionTemp>().Delete(question);
                }


                import.Status = (int)StatusEnum.Canceled;
                unitOfWork.Repository<Import>().Update(import);
                unitOfWork.SaveChanges();

                var listOption = unitOfWork.Repository<OptionTemp>().GetAll().Where(o => o.TempId == null).ToList();
                foreach (var option in listOption)
                {
                    unitOfWork.Repository<OptionTemp>().Delete(option);
                }
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

                var importModel = new ImportResultViewModel
                {
                    Id = import.Id,
                    Status = import.Status.Value,
                    CourseId = import.CourseId.Value,
                    NumberOfSuccess = import.TotalSuccess.HasValue ? import.TotalSuccess.Value : 0, //fix here
                    Questions = import.QuestionTemps.Select(q => new QuestionTempViewModel
                    {
                        Id = q.Id,
                        QuestionContent = q.QuestionContent,
                        Status = (StatusEnum)q.Status,
                        ImportId = importId,
                        Code = q.Code,
                        Message = q.Status == (int)StatusEnum.Invalid ? q.Message
                        : (q.DuplicatedString != null && q.DuplicatedString.Split(',').Count() > 1 ? $"It was duplicated with {q.DuplicatedString.Split(',').Count()} questions" : ""),
                        Image = q.Image,
                        IsInImportFile = q.DuplicateInImportId.HasValue,
                        Category = q.Category + " / " + q.LearningOutcome + " / " + q.LevelName,
                        Options = q.OptionTemps.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                            Image = o.Image
                        }).ToList(),
                        DuplicatedList = String.IsNullOrWhiteSpace(q.DuplicatedString) ? null : q.DuplicatedString.Split(',').Select(s => new DuplicatedQuestionViewModel
                        {
                            Id = int.Parse(s.Split('-')[0]),
                            IsBank = bool.Parse(s.Split('-')[1])
                        }).ToList()
                    }).OrderBy(q => q.Status).ToList(),
                };

                RemoveDuplicateGroup(importModel.Questions);

                foreach (var question in importModel.Questions.Where(q => q.DuplicatedList != null && q.DuplicatedList.Count == 2))
                {
                    if (question.DuplicatedList[0].IsBank)
                    {
                        var entity = unitOfWork.Repository<Question>().GetById(question.DuplicatedList[0].Id);
                        if (entity != null)
                        {
                            question.DuplicatedQuestion = new QuestionViewModel
                            {
                                Id = entity.Id,
                                Code = entity.QuestionCode,
                                Image = entity.Image,
                                CourseName = "Bank: " + entity.Course.Name,
                                QuestionContent = entity.QuestionContent,
                                Options = entity.Options.Select(o => new OptionViewModel
                                {
                                    OptionContent = o.OptionContent,
                                    IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                                }).ToList(),
                                IsBank = true,
                                IsAnotherImport = false
                            };
                        }


                    }
                    else
                    {
                        var entity = unitOfWork.Repository<QuestionTemp>().GetById(question.DuplicatedList[0].Id);
                        if (entity != null)
                        {
                            question.DuplicatedQuestion = new QuestionViewModel
                            {
                                Id = entity.Id,
                                Code = entity.Code,
                                CourseName = "Import file: ",
                                Image = entity.Image,
                                QuestionContent = entity.QuestionContent,
                                Options = entity.OptionTemps.Select(o => new OptionViewModel
                                {
                                    OptionContent = o.OptionContent,
                                    IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                                }).ToList(),
                                Status = (StatusEnum)entity.Status.Value,
                                IsBank = false,
                                IsAnotherImport = !(entity.ImportId == importId)
                            };
                        }

                    }
                }



                return importModel;

            }

            return null;
        }

        private void RemoveDuplicateGroup(List<QuestionTempViewModel> list)
        {
            List<string> duplicateGroup = new List<String>();
            foreach (var question in list.Where(q => q.DuplicatedList != null))
            {
                bool isInGroup = false;
                string duplicateString = ParseListDuplicateToString(question);
                foreach (string item in duplicateGroup)
                {
                    if (item.Equals(duplicateString))
                    {
                        isInGroup = true;
                        break;
                    }
                }

                if (!isInGroup)
                {
                    duplicateGroup.Add(duplicateString);
                }
                else
                {
                    question.IsHide = true;
                }
            }
        }

        public List<ImportViewModel> GetListImport(int? userId)
        {
            return unitOfWork.Repository<Import>().GetAll()
                .Where(im => userId.HasValue ? im.UserId == userId.Value : true)
                .Select(im => new ImportViewModel
                {
                    Id = im.Id,
                    Date = im.UpdatedDate.Value,
                    Status = (StatusEnum)im.Status.Value,
                    TotalQuestion = im.TotalQuestion.HasValue ? im.TotalQuestion.Value : 0,
                    TotalSuccess = im.TotalSuccess.HasValue ? im.TotalSuccess.Value : 0,
                    OwnerName = im.OwnerName
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
                    Code = questionTemp.Code,
                    QuestionContent = questionTemp.QuestionContent,
                    Status = (StatusEnum)questionTemp.Status,
                    ImportId = questionTemp.ImportId.Value,
                    Category = questionTemp.Category,
                    LearningOutcome = questionTemp.LearningOutcome,
                    Level = questionTemp.LevelName,
                    Images = questionTemp.Images.Select(i => new ImageViewModel
                    {
                        Source = i.Source
                    }).ToList(),
                    //DuplicatedQuestion = questionTemp.DuplicatedWithBank != null ? (new QuestionViewModel
                    //{
                    //    Id = questionTemp.DuplicatedWithBank.Id,
                    //    CourseName = questionTemp.DuplicatedWithBank.Course.Name,
                    //    Code = questionTemp.DuplicatedWithBank.QuestionCode,
                    //    QuestionContent = questionTemp.DuplicatedWithBank.QuestionContent,
                    //    Options = questionTemp.DuplicatedWithBank.Options.Select(o => new OptionViewModel
                    //    {
                    //        OptionContent = o.OptionContent,
                    //        IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                    //    }).ToList()
                    //}) : questionTemp.DuplicatedWithImport != null ? (new QuestionViewModel
                    //{
                    //    Id = questionTemp.DuplicatedWithImport.Id,
                    //    CourseName = "Import file",
                    //    Code = questionTemp.DuplicatedWithImport.Code,
                    //    QuestionContent = questionTemp.DuplicatedWithImport.QuestionContent,
                    //    Options = questionTemp.DuplicatedWithImport.OptionTemps.Select(o => new OptionViewModel
                    //    {
                    //        OptionContent = o.OptionContent,
                    //        IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                    //    }).ToList()
                    //}) : null,
                    Options = questionTemp.OptionTemps.Select(o => new OptionViewModel
                    {
                        Id = o.Id,
                        OptionContent = o.OptionContent,
                        Images = o.Images.Select(im => new ImageViewModel()
                        {
                            Source = im.Source
                        }).ToList(),
                        IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                    }).ToList()
                };
            }

            return null;
        }

        public async Task ImportToBank(int importId, bool checkSemantic = true)
        {
            using (var context = new QBCSContext())
            {
                string command = "EXEC CheckDuplicateImport @importId= @id, @checkSemantic=@semantic";
                await context.Database.ExecuteSqlCommandAsync(command, new SqlParameter("@id", importId), new SqlParameter("@semantic", checkSemantic ? 1 : 0));
            }
        }

        public async Task CheckDuplicateQuestion(int questionId, int logId)
        {
            using (var context = new QBCSContext())
            {
                string command = "EXEC CheckDuplicateQuestion @questionId=@qid, @logId=@lid";
                await context.Database.ExecuteSqlCommandAsync(command, new SqlParameter("@qid", questionId)
                                                                     , new SqlParameter("@lid", logId));
            }
        }

        public void UpdateQuestionTempWithTextarea(QuestionTempViewModel question)
        {
            var entity = unitOfWork.Repository<QuestionTemp>().GetById(question.Id);

            var checkedEntity = new List<QuestionTemp>();
            checkedEntity.Add(entity);
            checkedEntity = CheckRule(checkedEntity);
            entity = checkedEntity.FirstOrDefault();

            if (entity != null && (entity.Status == (int)StatusEnum.Editable
                                    || entity.Status == (int)StatusEnum.Invalid
                                    || entity.Status == (int)StatusEnum.Deleted
                                    || entity.Status == (int)StatusEnum.DeleteOrSkip))
            {
                entity.QuestionContent = question.QuestionContent;
                entity.Status = (int)StatusEnum.NotCheck;
                //entity.Image = question.Image;

                var listOptionEntity = entity.OptionTemps.ToList();

                foreach (var option in listOptionEntity)
                {
                    #region must fix
                    if (option.Images != null)
                    {
                        foreach (var image in option.Images.ToList())
                        {
                            unitOfWork.Repository<Image>().Delete(image);
                        }
                        unitOfWork.SaveChanges();

                    }
                    #endregion

                    unitOfWork.Repository<OptionTemp>().Delete(option);
                }
                if (entity.Images != null && entity.Images.Count > 0)
                {
                    foreach (var img in entity.Images.ToList())
                    {
                        unitOfWork.Repository<Image>().Delete(img);
                    }
                }

                question.Options = question.Options.Where(o => !String.IsNullOrWhiteSpace(o.OptionContent) && !o.OptionContent.Trim().ToLower().Equals("[html]")).ToList();
                //if (question.ImagesInput != null && question.ImagesInput.Count() > 0)
                //{
                //    entity.Images = question.ImagesInput.Select(im => new Image
                //    {
                //        Source = im
                //    }).ToList();
                //}

                foreach (var option in question.Options)
                {
                    unitOfWork.Repository<OptionTemp>().Insert(new OptionTemp
                    {
                        IsCorrect = option.IsCorrect,
                        OptionContent = option.OptionContent,
                        TempId = question.Id,
                        Images = option.Images.Select(i => new Image()
                        {
                            Source = i.Source
                        }).ToList()
                    });
                }
                foreach (var image in question.Images)
                {
                    unitOfWork.Repository<Image>().Insert(new Image
                    {
                        Source = image.Source,
                        QuestionTempId = question.Id
                    });
                }
                var list = new List<QuestionTemp>();
                list.Add(entity);
                list = CheckRule(list);
                unitOfWork.Repository<QuestionTemp>().Update(list.FirstOrDefault());
                unitOfWork.SaveChanges();
            }
        }

        public void UpdateQuestionTemp(QuestionTempViewModel question)
        {
            var entity = unitOfWork.Repository<QuestionTemp>().GetById(question.Id);

            var checkedEntity = new List<QuestionTemp>();
            checkedEntity.Add(entity);
            checkedEntity = CheckRule(checkedEntity);
            entity = checkedEntity.FirstOrDefault();

            if (entity != null && (entity.Status == (int)StatusEnum.Editable
                                    || entity.Status == (int)StatusEnum.Invalid
                                    || entity.Status == (int)StatusEnum.Deleted
                                    || entity.Status == (int)StatusEnum.DeleteOrSkip))
            {
                entity.QuestionContent = question.QuestionContent;
                entity.Status = (int)StatusEnum.NotCheck;
                //entity.Image = question.Image;

                var listOptionEntity = entity.OptionTemps.ToList();

                foreach (var option in listOptionEntity)
                {
                    #region must fix
                    if (option.Images != null)
                    {
                        foreach (var image in option.Images.ToList())
                        {
                            unitOfWork.Repository<Image>().Delete(image);
                        }
                        unitOfWork.SaveChanges();

                    }
                    #endregion

                    unitOfWork.Repository<OptionTemp>().Delete(option);
                }
                if (entity.Images != null && entity.Images.Count > 0)
                {
                    foreach (var img in entity.Images.ToList())
                    {
                        unitOfWork.Repository<Image>().Delete(img);
                    }
                }

                question.Options = question.Options.Where(o => !String.IsNullOrWhiteSpace(o.OptionContent) && !o.OptionContent.Trim().ToLower().Equals("[html]")).ToList();
                if (question.ImagesInput != null && question.ImagesInput.Count() > 0)
                {
                    entity.Images = question.ImagesInput.Select(im => new Image
                    {
                        Source = im
                    }).ToList();
                }

                foreach (var option in question.Options)
                {
                    unitOfWork.Repository<OptionTemp>().Insert(new OptionTemp
                    {
                        IsCorrect = option.IsCorrect,
                        OptionContent = option.OptionContent,
                        TempId = question.Id
                    });
                }
                var list = new List<QuestionTemp>();
                list.Add(entity);
                list = CheckRule(list);
                unitOfWork.Repository<QuestionTemp>().Update(list.FirstOrDefault());
                unitOfWork.SaveChanges();
            }
        }

        public List<QuestionTemp> CheckRule(List<QuestionTemp> tempQuestions)
        {
            if (tempQuestions == null)
            {
                return null;
            }
            var rules = unitOfWork.Repository<Rule>().GetAll().Where(r => r.IsDisable == false && r.IsUse == true).ToList();
            foreach (var tempQuestion in tempQuestions)
            {
                var checkCorrectOption = false;
                foreach (var option in tempQuestion.OptionTemps)
                {
                    if ((option.OptionContent.Equals("") || option.OptionContent.Trim().Equals("[html]")) && (option.Images == null || option.Images.Count() == 0))
                    {
                        tempQuestion.Status = (int)StatusEnum.Invalid;
                        tempQuestion.Message = "Option must not be empty";
                        break;
                    }
                    for (int i = 0; i < option.Images.Count - 1; i++)
                    {
                        for (int j = i + 1; j < option.Images.Count(); j++)
                        {
                            //checkDuplicateImage
                            if (CheckDuplicatedImage.CheckDuplicateImage(option.Images.ElementAtOrDefault(i).Source, option.Images.ElementAtOrDefault(i).Source))
                            {
                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                tempQuestion.Message = "Image in option must not be duplicate";
                                break;
                            }
                        }
                        if (tempQuestion.Status == (int)StatusEnum.Invalid)
                        {
                            break;
                        }
                    }
                    if ((bool)option.IsCorrect)
                    {
                        checkCorrectOption = true;
                        break;
                    }
                }
                if (!checkCorrectOption && tempQuestion.Status != (int)StatusEnum.Invalid)
                {
                    tempQuestion.Status = (int)StatusEnum.Invalid;
                    tempQuestion.Message = "Question must have a correct option";
                }

                if (tempQuestion.OptionTemps.Count > 1)
                {
                    for (int i = 0; i < tempQuestion.OptionTemps.Count - 1; i++)
                    {
                        for (int j = i + 1; j < tempQuestion.OptionTemps.Count; j++)
                        {
                            //var option1 = tempQuestion.OptionTemps.ElementAtOrDefault(i);
                            //var option2 = tempQuestion.OptionTemps.ElementAtOrDefault(j);
                            var trimOption1 = TrimOption(tempQuestion.OptionTemps.ElementAtOrDefault(i).OptionContent);
                            var trimOption2 = TrimOption(tempQuestion.OptionTemps.ElementAtOrDefault(j).OptionContent);
                            if ((!trimOption1.Equals("[html]") || !trimOption2.Equals("[html]")) && trimOption1.Equals(trimOption2))
                            {
                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                tempQuestion.Message = "All options must different from each others";
                                break;
                            }
                            else if (
                                (tempQuestion.OptionTemps.ElementAtOrDefault(i).Images != null && tempQuestion.OptionTemps.ElementAtOrDefault(i).Images.Count() > 0)
                                && (tempQuestion.OptionTemps.ElementAtOrDefault(j).Images != null && tempQuestion.OptionTemps.ElementAtOrDefault(j).Images.Count() > 0)
                            )
                            {
                                if (tempQuestion.OptionTemps.ElementAtOrDefault(i).Images.Count() == tempQuestion.OptionTemps.ElementAtOrDefault(j).Images.Count())
                                {
                                    int totalImage = tempQuestion.OptionTemps.ElementAtOrDefault(i).Images.Count();
                                    for (int k = 0; k < totalImage; k++)
                                    {
                                        for (int l = 0; l < totalImage; l++)
                                        {
                                            //checkDuplicateImage
                                            if (CheckDuplicatedImage.CheckDuplicateImage(
                                                tempQuestion.OptionTemps.ElementAtOrDefault(i).Images.ElementAtOrDefault(k).Source
                                                , tempQuestion.OptionTemps.ElementAtOrDefault(j).Images.ElementAtOrDefault(l).Source
                                            ))
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                                tempQuestion.Message = "All images in options must different from each others";
                                                break;
                                            }
                                        }
                                        if (tempQuestion.Status == (int)StatusEnum.Invalid)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (tempQuestion.Status == (int)StatusEnum.Invalid)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    tempQuestion.Status = (int)StatusEnum.Invalid;
                    break;
                }

                foreach (var rule in rules)
                {
                    if (tempQuestion.Status == (int)StatusEnum.Invalid)
                    {
                        break;
                    }
                    if (DateTime.Compare(DateTime.Now, (DateTime)rule.ActivateDate) >= 0)
                    {
                        switch (rule.KeyId)
                        {
                            //check min question length
                            case 1:
                                if (tempQuestion.QuestionContent.Length < int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                    tempQuestion.Message = "Question length must at least " + int.Parse(rule.Value) + " characters";
                                }
                                break;
                            //check max question length
                            case 2:
                                if (tempQuestion.QuestionContent.Length > int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                    tempQuestion.Message = "Question length can not exceed " + int.Parse(rule.Value) + " characters";
                                }
                                break;
                            //check banned words in question
                            case 3:
                                if (!rule.Value.Contains("·case_sensitive·"))
                                {
                                    var varRule = rule.Value.Replace("·case_sensitive·", "");
                                    var culture = CultureInfo.GetCultureInfo("en-GB");
                                    if (culture.CompareInfo.IndexOf(tempQuestion.QuestionContent, varRule, CompareOptions.IgnoreCase) >= 0)
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        tempQuestion.Message = "Question can not contain '" + (varRule) + "'";
                                    }
                                }
                                else
                                {
                                    var varRule = rule.Value.Replace("·case_sensitive·", "");
                                    if (tempQuestion.QuestionContent.Contains(varRule))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        tempQuestion.Message = "Question can not contain '" + (varRule) + "'";
                                    }
                                }

                                break;
                            //check min options count in question
                            case 4:
                                if (tempQuestion.OptionTemps.Count < int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                    tempQuestion.Message = "Number of options must at least " + int.Parse(rule.Value) + " options";
                                }
                                break;
                            //check max option count in question
                            case 5:
                                if (tempQuestion.OptionTemps.Count > int.Parse(rule.Value))
                                {
                                    tempQuestion.Status = (int)StatusEnum.Invalid;
                                    tempQuestion.Message = "Number of options can not exceed " + int.Parse(rule.Value) + " options";
                                }
                                break;
                            //check min option length
                            case 6:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if (option.OptionContent.Length < int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        tempQuestion.Message = "Option length must at least " + int.Parse(rule.Value) + " characters";
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
                                        tempQuestion.Message = "Option length can not exceed " + int.Parse(rule.Value) + " characters";
                                        break;
                                    }
                                }
                                break;
                            //check option length difference
                            case 8:
                                break;
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
                                            tempQuestion.Message = "Options can not contain '" + (varRule) + "'";
                                        }
                                    }
                                    else
                                    {
                                        var varRule = rule.Value.Replace("·case_sensitive·", "");
                                        if (option.OptionContent.Contains(varRule))
                                        {
                                            tempQuestion.Status = (int)StatusEnum.Invalid;
                                            tempQuestion.Message = "Options can not contain '" + (varRule) + "'";
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
                                        tempQuestion.Message = "Correct option length must at least " + int.Parse(rule.Value) + " characters";
                                        break;
                                    }
                                }
                                break;
                            //check max length in correct option
                            case 11:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((bool)option.IsCorrect && option.OptionContent.Length > int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        tempQuestion.Message = "Correct option length can not exceed " + int.Parse(rule.Value) + " characters";
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
                                                tempQuestion.Message = "Correct options can not contain '" + (varRule) + "'";
                                            }
                                        }
                                        else
                                        {
                                            var varRule = rule.Value.Replace("·case_sensitive·", "");
                                            if (option.OptionContent.Contains(varRule))
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                                tempQuestion.Message = "Correct options can not contain '" + (varRule) + "'";
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
                                        tempQuestion.Message = "Incorrect option length must at least " + int.Parse(rule.Value) + " characters";
                                        break;
                                    }
                                }
                                break;
                            //check max length in incorrect option
                            case 14:
                                foreach (var option in tempQuestion.OptionTemps)
                                {
                                    if ((!(bool)option.IsCorrect) && option.OptionContent.Length > int.Parse(rule.Value))
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        tempQuestion.Message = "Incorrect option length must not exceed " + int.Parse(rule.Value) + " characters";
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
                                                tempQuestion.Message = "Incorrect options must not contain '" + (varRule) + "'";
                                            }
                                        }
                                        else
                                        {

                                            var varRule = rule.Value.Replace("·case_sensitive·", "");
                                            if (option.OptionContent.Contains(varRule))
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                                tempQuestion.Message = "Incorrect options must not contain '" + (rule.Value) + "'";
                                            }
                                        }
                                    }
                                }
                                break;
                            //check allow longest correct option
                            case 16:
                                if (!rule.Value.Equals("True"))
                                {
                                    var compareOptions = new List<OptionTemp>();
                                    compareOptions = tempQuestion.OptionTemps.Select(o => new OptionTemp()
                                    {
                                        OptionContent = TrimTag(o.OptionContent),
                                        IsCorrect = o.IsCorrect
                                    }).ToList();
                                    var testOption = compareOptions.
                                                                OrderByDescending(o => o.OptionContent.Split(' ').Length).
                                                                ThenBy(o => o.IsCorrect).ToList();
                                    var varOption = testOption.First();
                                    if ((bool)varOption.IsCorrect)
                                    {
                                        tempQuestion.Status = (int)StatusEnum.Invalid;
                                        tempQuestion.Message = "Correct Option must not be the longest option";
                                    }
                                }
                                break;
                            //check allow shortest correct option
                            case 17:
                                if (!rule.Value.Equals("True"))
                                {
                                    var varOption = new OptionTemp();
                                    var rule8 = rules.Where(r => r.KeyId == 8).ToList();
                                    var compareOptions = new List<OptionTemp>();
                                    compareOptions = tempQuestion.OptionTemps.Select(o => new OptionTemp()
                                    {
                                        OptionContent = TrimTag(o.OptionContent),
                                        IsCorrect = o.IsCorrect
                                    }).ToList();
                                    var testOptions = compareOptions.
                                                                OrderBy(o => o.OptionContent.Split(' ').Length).
                                                                ThenBy(o => o.IsCorrect).ToList();

                                    if (rule8 != null && rule8.Count() > 0)
                                    {
                                        var value8 = int.Parse(rule8.FirstOrDefault().Value);
                                        var groupRules = testOptions.GroupBy(o => o.OptionContent.Split(' ').Length).ToList();
                                        if(groupRules.Count() > 1)
                                        {
                                            var shortest = groupRules[0].ToList().FirstOrDefault();
                                            var second = groupRules[1].ToList().FirstOrDefault();
                                            
                                        }
                                        else
                                        {
                                            varOption = testOptions.First();
                                            if ((bool)varOption.IsCorrect)
                                            {
                                                tempQuestion.Status = (int)StatusEnum.Invalid;
                                                tempQuestion.Message = "Correct Option must not be the shortest option";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        varOption = testOptions.First();
                                        if ((bool)varOption.IsCorrect)
                                        {
                                            tempQuestion.Status = (int)StatusEnum.Invalid;
                                            tempQuestion.Message = "Correct Option must not be the shortest option";
                                        }
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

        #region validate rule stuff
        private string TrimOption(string option)
        {
            if (option != null && !String.IsNullOrWhiteSpace(option))
            {
                option = option.Replace("  ", " ");
                if (option.Last().ToString().Equals("."))
                {
                    option.Remove(option.Length - 1);
                }
            }

            //option = option.Replace(",", "");
            return option;
        }

        private string TrimTag(string str)
        {
            str = str.Replace("<cbr>", " ");
            str = str.Replace("<br>", " ");
            str = str.Replace("<br/>", " ");
            str = str.Replace("<br />", " ");
            str = str.Replace("<br>", " ");
            str = str.Replace("<sub>", " ");
            str = str.Replace("</sub>", " ");
            str = str.Replace("<p>", " ");
            str = str.Replace("</p>", " ");
            str = str.Replace("</sup>", " ");
            str = str.Replace("<sup>", " ");
            str = str.Replace("<span>", " ");
            str = str.Replace("</span>", " ");
            str = str.Replace("</b>", " ");
            str = str.Replace("<b>", " ");
            str = str.Replace("</u>", " ");
            str = str.Replace("<u>", " ");
            str = str.Replace("</i>", " ");
            str = str.Replace("<i>", " ");
            str = str.Replace("[html]", " ");
            str = Regex.Replace(str, @"\s{2,}", " ");
            return str;
        }

        #endregion
        public void UpdateQuestionTempStatus(int questionTempId, int status)
        {
            var questionTemp = unitOfWork.Repository<QuestionTemp>().GetById(questionTempId);

            if (questionTemp.Status != status)
            {
                if (status == (int)StatusEnum.Deleted)
                {
                    questionTemp.OldStatus = questionTemp.Status;
                }
                questionTemp.Status = status;
                unitOfWork.Repository<QuestionTemp>().Update(questionTemp);
                unitOfWork.SaveChanges();
            }
        }

        public QuestionTempViewModel GetDuplicatedDetail(int questionTempId)
        {

            var entity = unitOfWork.Repository<QuestionTemp>().GetById(questionTempId);
            if (entity != null)
            {

                QuestionTempViewModel model = new QuestionTempViewModel()
                {
                    Id = entity.Id,
                    QuestionContent = entity.QuestionContent,
                    Status = (StatusEnum)entity.Status,
                    ImportId = entity.ImportId.Value,
                    Code = entity.Code,
                    Images = entity.Images.Select(i => new ImageViewModel
                    {
                        Source = i.Source
                    }).ToList(),
                    Category = entity.Category + " / " + entity.LearningOutcome + " / " + entity.LevelName,
                    Options = entity.OptionTemps.Select(o => new OptionViewModel
                    {
                        Id = o.Id,
                        OptionContent = o.OptionContent,
                        Images = o.Images.Select(oi => new ImageViewModel()
                        {
                            Source = oi.Source
                        }).ToList(),
                        IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                    }).ToList()
                };

                var listDuplicated = entity.DuplicatedString.Split(',').Select(d => new DuplicatedQuestionViewModel
                {
                    Id = int.Parse(d.Split('-')[0]),
                    IsBank = bool.Parse(d.Split('-')[1])
                }).ToList();

                foreach (var duplicated in listDuplicated)
                {
                    if (duplicated.IsBank)
                    {
                        var questionEntity = unitOfWork.Repository<Question>().GetById(duplicated.Id);
                        if (questionEntity != null)
                        {
                            duplicated.Code = questionEntity.QuestionCode;
                            duplicated.QuestionContent = questionEntity.QuestionContent;
                            duplicated.Options = questionEntity.Options.Select(o => new OptionViewModel
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                                Images = o.Images.Select(oi => new ImageViewModel()
                                {
                                    Source = oi.Source
                                }).ToList()
                            }).ToList();
                            duplicated.Images = questionEntity.Images.Select(i => new ImageViewModel
                            {
                                Source = i.Source
                            }).ToList();
                            duplicated.IsAnotherImport = false;
                        }

                    }
                    else
                    {
                        var questionEntity = unitOfWork.Repository<QuestionTemp>().GetById(duplicated.Id);
                        if (questionEntity != null)
                        {
                            duplicated.Code = questionEntity.Code;
                            duplicated.QuestionContent = questionEntity.QuestionContent;
                            duplicated.Options = questionEntity.OptionTemps.Select(o => new OptionViewModel
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                                Images = o.Images.Select(oi => new ImageViewModel()
                                {
                                    Source = oi.Source
                                }).ToList(),
                            }).ToList();
                            duplicated.Images = questionEntity.Images.Select(i => new ImageViewModel
                            {
                                Source = i.Source
                            }).ToList();
                            duplicated.Status = questionEntity.Status.HasValue ? (StatusEnum)questionEntity.Status.Value : 0;
                            duplicated.IsAnotherImport = !(questionEntity.ImportId == entity.ImportId);
                        }

                    }
                }

                model.DuplicatedList = listDuplicated.Where(q => q.Options != null).ToList();

                return model;

            }

            return null;

        }

        public void RecoveryQuestionTemp(int questionTempId)
        {
            var questionTemp = unitOfWork.Repository<QuestionTemp>().GetById(questionTempId);
            if (questionTemp.OldStatus != null)
            {
                questionTemp.Status = questionTemp.OldStatus;
            }
            unitOfWork.Repository<QuestionTemp>().Update(questionTemp);
            unitOfWork.SaveChanges();
        }

        private string ParseListDuplicateToString(QuestionTempViewModel temp)
        {
            temp.DuplicatedList.Add(new DuplicatedQuestionViewModel
            {
                Id = temp.Id,
                IsBank = false
            });
            return String.Join(",", temp.DuplicatedList.OrderBy(t => t.Id).Select(s => $"{s.Id}-{s.IsBank}").ToArray());
        }

        public List<QuestionTempViewModel> GetListQuestionTempByStatus(int importId, int status)
        {
            var list = unitOfWork.Repository<QuestionTemp>().GetAll()
              .Where(q => q.ImportId == importId && q.Status == status)
              .ToList()
              .Select(q => new QuestionTempViewModel
              {
                  Id = q.Id,
                  QuestionContent = q.QuestionContent,
                  Status = (StatusEnum)q.Status,
                  ImportId = importId,
                  Code = q.Code,
                  Message = q.Status == (int)StatusEnum.Invalid ? q.Message
                      : (q.DuplicatedString != null && q.DuplicatedString.Split(',').Count() > 1 ? $"It was duplicated with {q.DuplicatedString.Split(',').Count()} questions" : ""),
                  Image = q.Image,
                  IsInImportFile = q.DuplicateInImportId.HasValue,
                  Category = q.Category + " / " + q.LearningOutcome + " / " + q.LevelName,
                  Options = q.OptionTemps.Select(o => new OptionViewModel
                  {
                      OptionContent = o.OptionContent,
                      IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                      Image = o.Image
                  }).ToList(),
                  DuplicatedList = String.IsNullOrWhiteSpace(q.DuplicatedString) ? null : q.DuplicatedString.Split(',').Select(s => new DuplicatedQuestionViewModel
                  {
                      Id = int.Parse(s.Split('-')[0]),
                      IsBank = bool.Parse(s.Split('-')[1])
                  }).ToList()
              }).ToList();
            RemoveDuplicateGroup(list);
            foreach (var question in list.Where(q => q.DuplicatedList != null && q.DuplicatedList.Count == 2))
            {
                if (question.DuplicatedList[0].IsBank)
                {
                    var entity = unitOfWork.Repository<Question>().GetById(question.DuplicatedList[0].Id);
                    question.DuplicatedQuestion = new QuestionViewModel
                    {
                        Id = entity.Id,
                        Code = entity.QuestionCode,
                        CourseName = "Bank: " + entity.Course.Name,
                        QuestionContent = entity.QuestionContent,
                        Options = entity.Options.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                        }).ToList(),
                        IsBank = true,
                        IsAnotherImport = false
                    };

                }
                else
                {
                    var entity = unitOfWork.Repository<QuestionTemp>().GetById(question.DuplicatedList[0].Id);
                    question.DuplicatedQuestion = new QuestionViewModel
                    {
                        Id = entity.Id,
                        Code = entity.Code,
                        CourseName = "Import file: ",
                        QuestionContent = entity.QuestionContent,
                        Options = entity.OptionTemps.Select(o => new OptionViewModel
                        {
                            OptionContent = o.OptionContent,
                            IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                        }).ToList(),
                        Status = (StatusEnum)entity.Status.Value,
                        IsBank = false,
                        IsAnotherImport = !(entity.ImportId == importId)
                    };
                }
            }
            return list;

        }


    }
}
