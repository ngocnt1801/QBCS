using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace QBCS.Service.Implement
{
    public class QuestionService : IQuestionService
    {
        private IUnitOfWork unitOfWork;
        private IImportService importService;

        public QuestionService()
        {
            unitOfWork = new UnitOfWork();
            importService = new ImportService();
        }

        public bool Add(QuestionViewModel question)
        {
            bool result = false;
            if (!IsDuplicateQuestion(question))
            {
                var entity = new Question()
                {
                    QuestionContent = question.QuestionContent,
                    CourseId = question.CourseId,
                    IsDisable = false,
                    Options = question.Options.Select(o => new Option()
                    {
                        IsCorrect = o.IsCorrect,
                        OptionContent = o.OptionContent
                    }).ToList(),
                    Priority = 0,
                    Frequency = 0
                };

                unitOfWork.Repository<Question>().Insert(entity);
                unitOfWork.SaveChanges();
                result = true;
            }

            return result;

        }

        private bool IsDuplicateQuestion(QuestionViewModel question)
        {
            bool result = false;
            //***
            //code check duplicate here
            //***

            return result;
        }

        public List<QuestionViewModel> GetQuestionsByCourse(int CourseId)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            List<Question> QuestionsByCourse = questions.Where(q => q.CourseId == CourseId).ToList();

            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();

            foreach (var ques in QuestionsByCourse)
            {
                List<OptionViewModel> optionViewModels = new List<OptionViewModel>();
                foreach (var option in ques.Options)
                {

                    OptionViewModel optionViewModel = new OptionViewModel()
                    {
                        Id = option.Id,
                        OptionContent = option.OptionContent,
                        IsCorrect = (bool)option.IsCorrect
                    };
                    optionViewModels.Add(optionViewModel);
                }


                QuestionViewModel questionViewModel = ParseEntityToModel(ques, optionViewModels);
                questionViewModels.Add(questionViewModel);
            }
            return questionViewModels;
        }

        public QuestionViewModel GetQuestionById(int id)
        {
            Question QuestionById = unitOfWork.Repository<Question>().GetById(id);

            List<OptionViewModel> optionViewModels = new List<OptionViewModel>();
            foreach (var option in QuestionById.Options)
            {

                OptionViewModel optionViewModel = new OptionViewModel()
                {
                    Id = option.Id,
                    OptionContent = option.OptionContent,
                    IsCorrect = (bool)option.IsCorrect
                };
                optionViewModels.Add(optionViewModel);
            }


            QuestionViewModel questionViewModel = ParseEntityToModel(QuestionById, optionViewModels);
            return questionViewModel;
        }
        public List<QuestionViewModel> GetQuestionByQuestionId(int questionId)
        {
            var question = unitOfWork.Repository<Question>().GetById(questionId);

            var questions = question.Options.Select(c => new QuestionViewModel
            {
                Id = (int)c.QuestionId,
                QuestionContent = c.Question.QuestionContent,
                Options = c.Question.Options.Select(d => new OptionViewModel
                {
                    Id = d.Id,
                    OptionContent = d.OptionContent,
                    IsCorrect = (bool)d.IsCorrect
                }).ToList()
            }).ToList();

            return questions;
        }

        public bool UpdateQuestion(QuestionViewModel question)
        {
            Question questionById = unitOfWork.Repository<Question>().GetById(question.Id);
            questionById.QuestionContent = question.QuestionContent;
            questionById.LevelId = question.LevelId;
            if (question.LearningOutcomeId != 0)
            {
                questionById.LearningOutcomeId = question.LearningOutcomeId;
            }

            if (question.TopicId != 0)
            {
                questionById.TopicId = question.TopicId;
            }

            unitOfWork.Repository<Question>().Update(questionById);
            unitOfWork.SaveChanges();
            return true;
        }

        private QuestionViewModel ParseEntityToModel(Question question, List<OptionViewModel> options)
        {
            QuestionViewModel questionViewModel = new ViewModel.QuestionViewModel()
            {
                Id = question.Id,
                QuestionContent = question.QuestionContent,
                Options = options
            };
            if (question.CourseId != null)
            {
                questionViewModel.CourseId = (int)question.CourseId;
            }
            if (question.TopicId != null)
            {
                questionViewModel.TopicId = (int)question.TopicId;
            }
            if (question.LevelId != null)
            {
                questionViewModel.LevelId = (int)question.LevelId;
            }
            if (question.LearningOutcomeId != null)
            {
                questionViewModel.LearningOutcomeId = (int)question.LearningOutcomeId;
            }

            return questionViewModel;
        }

        public List<Question> GetQuestionsByContent(string questionContent)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll().Where(q => q.QuestionContent.Contains(questionContent));
            List<Question> result = questions.ToList();
            return result;
        }

        public List<Question> GetQuestionSearchBar(string searchInput)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll().Where(q =>
                                                                                            q.QuestionContent.Contains(searchInput) ||
                                                                                            q.Course.Name.Contains(searchInput))
                                                                                            .Take(5);
            List<Question> result = questions.ToList();
            return result;
        }
        public List<QuestionViewModel> GetAllQuestionByCourseId(int courseId)
        {
            var course = unitOfWork.Repository<Course>().GetById(courseId);
            List<QuestionViewModel> questions = course.Questions.Select(c => new QuestionViewModel
            {
                CourseId = (int)c.CourseId,
                CourseCode = c.Course.Code,
                CourseName = c.Course.Name,
                QuestionContent = c.QuestionContent,
                Options = c.Options.Select(d => new OptionViewModel
                {
                    Id = d.Id,
                    OptionContent = d.OptionContent,
                    IsCorrect = (bool)d.IsCorrect
                }).ToList()
            }).ToList();
            return questions;
        }
        public List<QuestionViewModel> GetAllQuestions()
        {
            List<QuestionViewModel> questions = unitOfWork.Repository<Question>().GetAll().Select(c => new QuestionViewModel
            {
                CourseId = (int)c.CourseId,
                CourseCode = c.Course.Code,
                CourseName = c.Course.Name,
                QuestionContent = c.QuestionContent,
                Options = c.Options.Select(d => new OptionViewModel
                {
                    Id = d.Id,
                    OptionContent = d.OptionContent,
                    IsCorrect = (bool)d.IsCorrect
                }).ToList()
            }).ToList();
            return questions;
        }

        public List<QuestionViewModel> CheckDuplicated()
        {
            //
            //check here
            //

            //this is fake data
            var result = unitOfWork.Repository<Question>().GetAll().Select(c => new QuestionViewModel
            {
                CourseId = (int)c.CourseId,
                CourseCode = c.Course.Code,
                CourseName = c.Course.Name,
                Id = c.Id,
                QuestionContent = c.QuestionContent,
                Options = c.Options.Select(d => new OptionViewModel
                {
                    Id = d.Id,
                    OptionContent = d.OptionContent,
                    IsCorrect = (bool)d.IsCorrect
                }).ToList()
            }).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                if (i % 2 == 0)
                {
                    result[i].IsDuplicated = true;
                    result[i].DuplicatedQuestion = result[i];
                }
                else
                {
                    result[i].IsDuplicated = false;
                }
            }


            return result;
        }
        static string category = null;
        static string level = null;
        static string topic = null;
        public bool InsertQuestion(HttpPostedFileBase questionFile, int userId, int courseId)
        {

            bool check = false;
            StreamReader reader = null;
            List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();
            var import = new Import();
            StringBuilder sb = new StringBuilder();

            try
            {
                string extensionFile = Path.GetExtension(questionFile.FileName);
                #region process xml
                if (extensionFile.Equals(".xml"))
                {
                    List<QuestionTmpModel> listQuestionXml = new List<QuestionTmpModel>();
                    XmlSerializer xmlSer = new XmlSerializer(typeof(quiz));
                    reader = new StreamReader(questionFile.InputStream);
                    quiz questionXml = (quiz)xmlSer.Deserialize(reader);
                    List<OptionTemp> tempAns = new List<OptionTemp>();
                    QuestionTmpModel question = new QuestionTmpModel();
                    OptionTemp option = new OptionTemp();

                    for (int i = 0; i < questionXml.question.Count(); i++)
                    {
                        string questionContent = null;
                        string rightAnswer = null;
                        string wrongAnswer = null;
                        string temp = null;

                        #region get category
                        if (questionXml.question[i].category != null)
                        {
                            temp = questionXml.question[i].category.text.ToString();
                            string[] arrListStr = temp.Split('/');
                            for (int z = 0; z < arrListStr.Length; z++)
                            {
                                if (z == 1)
                                {
                                    category = "";
                                    category = arrListStr[z];
                                }
                                if (z == 2)
                                {
                                    topic = "";
                                    topic = arrListStr[z];
                                }
                                if (z == 3)
                                {
                                    level = "";
                                    level = arrListStr[z];
                                }
                            }
                        }
                        #endregion
                        if (questionXml.question[i].questiontext != null)
                        {
                            
                            string tempParser = "";
                            string checkHTML = questionXml.question[i].questiontext.format.ToString();
                            tempParser = questionXml.question[i].questiontext.text;
                            // sb.Append("Question " + questionXml.question[i].questiontext.text);
                            questionContent = WebUtility.HtmlDecode(tempParser);
                            questionContent = StringProcess.RemoveHtmlTag(questionContent);
                            //questionContent = StringProcess.RemoveTag(questionContent, @"\n", @"<cbr>");
                            if (checkHTML.Equals("html"))
                            {
                                question.QuestionContent = "[html]"+ questionContent;
                            }
                            else
                            {
                                question.QuestionContent = questionContent;
                            }
                            question.Code = questionXml.question[i].name.text.ToString();                          
                            if (category != null)
                            {
                                question.Category = category.Trim();
                                question.Level = level.Trim();
                                question.Topic = topic.Trim();
                            }
                            tempParser = "";

                            #region get question, option
                            if (questionXml.question[i].answer != null)
                            {
                                for (int j = 0; j < questionXml.question[i].answer.Count(); j++)
                                {
                                    if (questionXml.question[i].answer[j].fraction.ToString().Equals("100"))
                                    {
                                        tempParser = questionXml.question[i].answer[j].text;
                                        rightAnswer = WebUtility.HtmlDecode(tempParser);
                                        rightAnswer = StringProcess.RemoveHtmlTag(rightAnswer);
                                        //rightAnswer = StringProcess.RemoveTag(rightAnswer, @"\n", @"<cbr>");
                                        option = new OptionTemp();
                                        option.OptionContent = rightAnswer;
                                        option.IsCorrect = true;
                                        tempAns.Add(option);
                                        tempParser = "";
                                    }
                                    else
                                    if (questionXml.question[i].answer[j].fraction.ToString().Equals("0"))
                                    {
                                        tempParser = questionXml.question[i].answer[j].text;
                                        wrongAnswer = WebUtility.HtmlDecode(tempParser);
                                        wrongAnswer = StringProcess.RemoveHtmlTag(wrongAnswer);
                                        //wrongAnswer = StringProcess.RemoveTag(wrongAnswer, @"\n", @"<cbr>");
                                        option = new OptionTemp();
                                        option.OptionContent = wrongAnswer;
                                        option.IsCorrect = false;
                                        tempAns.Add(option);
                                        tempParser = "";
                                    }

                                }
                            }
                            #endregion
                        }

                        if (question.QuestionContent != null)
                        {
                            listQuestionXml.Add(question);
                            if (listQuestionXml.Count() > 0 && tempAns.Count() > 0)
                            {
                                DateTime importTime = DateTime.Now;
                                import.QuestionTemps.Add(new QuestionTemp()
                                {
                                    QuestionContent = question.QuestionContent,
                                    Status = (int)StatusEnum.NotCheck,
                                    Code = question.Code,
                                    Category = question.Category,
                                    Topic = question.Topic,
                                    LevelName = question.Level,  
                                    OptionTemps = tempAns.Select(o => new OptionTemp()
                                    {
                                        OptionContent = o.OptionContent,
                                        IsCorrect = o.IsCorrect
                                    }).ToList()

                                });
                                import.ImportedDate = DateTime.Now;
                                import.UserId = userId;
                                
                               
                            }
                            int z = 0;
                            foreach (var item in listQuestionXml)
                            {
                                z++;
                                sb.AppendLine(z + "");
                                sb.AppendLine("Question " + item.QuestionContent);
                                sb.AppendLine("Code " + item.Code + "\n");
                                sb.AppendLine();
                                File.AppendAllText(@"E:\Capstone\log\" + "logXML.txt", sb.ToString());
                                sb.Clear();
                            }
                            listQuestionXml = new List<QuestionTmpModel>();
                            question = new QuestionTmpModel();
                            tempAns = new List<OptionTemp>();
                        }
                    }

                }
                #endregion

                #region process gift
                if (extensionFile.Equals(".txt"))
                {
                    GIFTUtilities ulti = new GIFTUtilities();
                    QuestionTemp quesTmp = new QuestionTemp();
                    reader = new StreamReader(questionFile.InputStream, Encoding.UTF8);
                    listQuestion = ulti.StripTagsCharArray(reader);
                    DateTime importTime = DateTime.Now;
                    import = new Import()
                    {
                        CourseId = courseId,
                        UserId = userId,
                        TotalQuestion = listQuestion.Count(),
                        QuestionTemps = listQuestion.Select(q => new QuestionTemp()
                        {
                            QuestionContent = q.QuestionContent,
                            Code = q.Code,
                            Status = (int)StatusEnum.NotCheck,
                            Category = q.Category,
                            Topic = q.Topic,
                            LevelName = q.Level,
                            OptionTemps = q.Options.Select(o => new OptionTemp()
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect
                            }).ToList(),
                        }).ToList(),
                        ImportedDate = importTime
                    };
                    int g = 0;
                    foreach (var item in listQuestion)
                    {
                        g++;
                        sb.AppendLine(g + "");
                        sb.AppendLine("Question: " + item.QuestionContent);
                        sb.AppendLine("Code: " + item.Code + "\n");
                        sb.AppendLine();
                        File.AppendAllText(@"E:\Capstone\log\" + "logGIFT.txt", sb.ToString());
                        sb.Clear();
                    }
                }
                #endregion
                if (import.QuestionTemps.Count() > 0)
                {
                    import.Status = (int)StatusEnum.NotCheck;
                    import.CourseId = courseId;
                    //check formats
                    import.QuestionTemps = importService.CheckRule(import.QuestionTemps.ToList());
                    var entity = unitOfWork.Repository<Import>().InsertAndReturn(import);
                    import.TotalQuestion = import.QuestionTemps.Count();
                    unitOfWork.SaveChanges();

                    //call store check duplicate
                    Task.Factory.StartNew(() =>
                    {
                        importService.ImportToBank(entity.Id);
                    });
                    check = true;
                }
                else
                {
                    // return user have to import file
                }

            }
            //catch (Exception ex)
            //{
            //    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            //    //Console.WriteLine(ex.Message);
            //}
            finally
            {
                reader.Close();
            }

            return check;
        }

        public int GetMinFreQuencyByTopicAndLevel(int topicId, int levelId)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            Question question = questions.Where(q => q.TopicId == topicId && q.LevelId == levelId).OrderBy(q => q.Frequency).Take(1).FirstOrDefault();

            return (int)question.Frequency;
        }
        public int GetMinFreQuencyByLearningOutcome(int learningOutcomeId, int levelId)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            Question question = questions.Where(q => q.LearningOutcomeId == learningOutcomeId && q.LevelId == levelId).OrderBy(q => q.Frequency).Take(1).FirstOrDefault();
            return (int)question.Frequency;
        }
        public List<QuestionViewModel> GetQuestionList(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId)
        {
            var result = unitOfWork.Repository<Question>().GetAll().Where(q => !q.IsDisable.HasValue || !q.IsDisable.Value);

            if (courseId != null && courseId != 0)
            {
                result = result.Where(q => q.CourseId == courseId);
            }

            if (categoryId != null && categoryId != 0)
            {
                result = result.Where(q => q.CategoryId == categoryId);
            } else if (categoryId == 0)
            {
                result = result.Where(q => q.CategoryId == null);
            }

            if (learningoutcomeId != null && learningoutcomeId != 0)
            {
                result = result.Where(q => q.LearningOutcomeId == learningoutcomeId);
            }

            if (topicId != null && topicId != 0)
            {
                result = result.Where(q => q.TopicId == topicId);
            }

            if (levelId != null && levelId != 0)
            {
                result = result.Where(q => q.LevelId == levelId);
            }

            return result.Select(q => new QuestionViewModel
            {
                Id = q.Id,
                Code = q.QuestionCode,
                QuestionContent = q.QuestionContent,
                Options = q.Options.Select(o => new OptionViewModel
                {
                    Id = o.Id,
                    OptionContent = o.OptionContent,
                    IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value
                }).ToList(),
                IsDisable = q.IsDisable.HasValue && q.IsDisable.Value
            })
            .OrderByDescending(q => !q.IsDisable)
            .ToList();
        }

        public void ToggleDisable(int id)
        {
            var entity = unitOfWork.Repository<Question>().GetById(id);
            if (entity != null)
            {
                entity.IsDisable = entity.IsDisable.HasValue ? !entity.IsDisable : true;
            }
            unitOfWork.Repository<Question>().Update(entity);
            unitOfWork.SaveChanges();
        }

        public void UpdateCategory(int[] questionIds, int? categoryId, int? learningOutcomeId, int? levelId)
        {
            if (questionIds != null)
            {
                var entityList = unitOfWork.Repository<Question>().GetAll().Where(q => questionIds.Contains(q.Id)).ToList();

                foreach (var entity in entityList)
                {
                    if (categoryId != null && categoryId != 0)
                    {
                        entity.CategoryId = categoryId;
                    }
                    else
                    {
                        entity.CategoryId = null;
                    }

                    if (learningOutcomeId != null && learningOutcomeId != 0)
                    {
                        entity.TopicId = learningOutcomeId; // fix here
                    }
                    else
                    {
                        entity.TopicId = null;
                    }

                    if (levelId != null && levelId != 0)
                    {
                        entity.LevelId = levelId;
                    }
                    else
                    {
                        entity.LevelId = null;
                    }
                    unitOfWork.Repository<Question>().Update(entity);
                }

                unitOfWork.SaveChanges();
            }

        }
    }
}
