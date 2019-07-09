using HtmlAgilityPack;
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
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace QBCS.Service.Implement
{
    public class QuestionService : IQuestionService
    {
        private IUnitOfWork unitOfWork;
        private IImportService importService;
        private ILogService logService;

        public QuestionService()
        {
            unitOfWork = new UnitOfWork();
            importService = new ImportService();
            logService = new LogService();
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
            //Question questionById = unitOfWork.Repository<Question>().GetById(question.Id);
            //questionById.QuestionContent = question.QuestionContent;

            //if (question.LevelId != 0)
            //{
            //    questionById.LevelId = question.LevelId;
            //}
            //if (question.LearningOutcomeId != 0)
            //{
            //    questionById.LearningOutcomeId = question.LearningOutcomeId;
            //}

            //unitOfWork.Repository<Question>().Update(questionById);
            //unitOfWork.SaveChanges();

            string quesTemp = "";
            QuestionTemp entity = new QuestionTemp();
            entity.UpdateQuestionId = question.Id;
            if (question.QuestionContent != null)
            {
                quesTemp = WebUtility.HtmlDecode(question.QuestionContent);
            }
            entity.QuestionContent = quesTemp;
            entity.Type = (int) TypeEnum.Update;
            entity.LearningOutcome = question.LearningOutcomeId != 0 ? question.LearningOutcomeId.ToString() : "";
            entity.LevelName = question.LevelId != 0 ? question.LevelId.ToString() : "";
            entity.Category = question.CategoryId != 0 ? question.CategoryId.ToString() : "";

            entity.OptionTemps = question.Options.Select(o => new OptionTemp()
            {
                OptionContent = WebUtility.HtmlDecode(o.OptionContent),
                IsCorrect = o.IsCorrect,
                UpdateOptionId = o.Id
            }).ToList();

            var listEntity = new List<QuestionTemp>();
            listEntity.Add(entity);
            listEntity = importService.CheckRule(listEntity);

            var tmp = unitOfWork.Repository<QuestionTemp>().InsertAndReturn(listEntity.FirstOrDefault());
            unitOfWork.SaveChanges();

            //get log id
            var logEntity = unitOfWork.Repository<Log>().GetAll()
                                    .Where(l => l.TargetId == entity.UpdateQuestionId)// add check disable here after merge with Nhi
                                    .OrderByDescending(l => l.Date).FirstOrDefault();
            if (logEntity != null)
            {
                //call store check duplicate
                Task.Factory.StartNew(() =>
                {
                    importService.CheckDuplicateQuestion(tmp.Id, logEntity.Id);
                });
            }

            return true;
        }

        private QuestionViewModel ParseEntityToModel(Question question, List<OptionViewModel> options)
        {
            QuestionViewModel questionViewModel = new ViewModel.QuestionViewModel()
            {
                Id = question.Id,
                QuestionCode = question.QuestionCode,
                QuestionContent = question.QuestionContent,
                Options = options,
                Image = question.Image,
                ImportId = (int)question.ImportId
            };
            if (question.Image != null)
            {
                questionViewModel.Image = question.Image;
            }
            if (question.CourseId != null)
            {
                questionViewModel.CourseId = (int)question.CourseId;
            }
            if (question.CategoryId != null)
            {
                questionViewModel.CategoryId = (int)question.CategoryId;
            }
            if (question.LevelId != null)
            {
                questionViewModel.LevelId = (int)question.LevelId;
            }
            if (question.LearningOutcomeId != null)
            {
                questionViewModel.LearningOutcomeId = (int)question.LearningOutcomeId;
            }
            if (question.Course != null)
            {
                questionViewModel.CourseName = question.Course.Name;
            }
            if (question.LearningOutcome != null)
            {
                questionViewModel.LearningOutcomeName = question.LearningOutcome.Name;
            }
            if (question.Level != null)
            {
                questionViewModel.LevelName = question.Level.Name;
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
                ImportId = (int)c.ImportId,
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
            //var result = unitOfWork.Repository<Question>().GetAll().Select(c => new QuestionViewModel
            //{
            //    CourseId = (int)c.CourseId,
            //    CourseCode = c.Course.Code,
            //    CourseName = c.Course.Name,
            //    Id = c.Id,
            //    QuestionContent = c.QuestionContent,
            //    Options = c.Options.Select(d => new OptionViewModel
            //    {
            //        Id = d.Id,
            //        OptionContent = d.OptionContent,
            //        IsCorrect = (bool)d.IsCorrect
            //    }).ToList()
            //}).ToList();
            //for (int i = 0; i < result.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        result[i].IsDuplicated = true;
            //        result[i].DuplicatedQuestion = result[i];
            //    }
            //    else
            //    {
            //        result[i].IsDuplicated = false;
            //    }
            //}


            return null;
        }

        public bool InsertQuestion(HttpPostedFileBase questionFile, int userId, int courseId, bool checkCate, bool checkHTML, string ownerName, string prefix = "")
        {
            string category = "";
            string level = "";
            string learningOutcome = "";
            bool check = false;
            StreamReader reader = null;
            List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();
            var import = new Import();

            string checkHTMLTemp = "";
            int countLog = 0;
            HtmlDocument htmlDoc = new HtmlDocument();
            var userName = (UserViewModel)HttpContext.Current.Session["user"];
            string user = userName.Fullname.ToString();
            try
            {
                StringProcess stringProcess = new StringProcess();
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
                        int status = (int)StatusEnum.NotCheck;
                        #region get category
                        if (questionXml.question[i].category != null && checkCate == true)
                        {
                            temp = questionXml.question[i].category.text.ToString();
                            string[] arrListStr = temp.Split('/');
                            for (int z = 0; z < arrListStr.Length; z++)
                            {
                                if (z == 1)
                                {
                                    category = "";
                                    if (arrListStr[z] != null)
                                    {
                                        category = arrListStr[z];
                                    }

                                }
                                if (z == 2)
                                {
                                    learningOutcome = "";
                                    if (arrListStr[z] != null)
                                    {
                                        learningOutcome = arrListStr[z];
                                    }

                                }
                                if (z == 3)
                                {
                                    level = "";
                                    if (arrListStr[z] != null)
                                    {
                                        level = arrListStr[z];
                                    }

                                }

                            }
                            continue;
                        }
                        #endregion
                        if (questionXml.question[i].questiontext != null)
                        {

                            string tempParser = "";
                            string file = "";
                            checkHTMLTemp = questionXml.question[i].questiontext.format.ToString();
                            tempParser = questionXml.question[i].questiontext.text;

                            if (questionXml.question[i].questiontext.file != null)
                            {
                                if (questionXml.question[i].questiontext.file.Value != null)
                                {
                                    file = questionXml.question[i].questiontext.file.Value.ToString();
                                    question.Image = file;
                                    status = (int)Enum.StatusEnum.NotCheck;
                                }

                            }

                            // sb.Append("Question " + questionXml.question[i].questiontext.text);
                            tempParser = stringProcess.RemoveHtmlBrTag(tempParser);

                            if (checkHTML == false)
                            {
                                htmlDoc.LoadHtml(tempParser);
                                tempParser = htmlDoc.DocumentNode.InnerText;
                            }
                            questionContent = WebUtility.HtmlDecode(tempParser);
                            questionContent = stringProcess.RemoveHtmlTagXML(questionContent);
                            questionContent = stringProcess.UpperCaseKeyWord(questionContent);
                            if (checkHTML.Equals("html"))
                            {
                                question.QuestionContent = "[html]" + questionContent;
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
                                question.LearningOutcome = learningOutcome.Trim();
                            }
                            tempParser = "";

                            #region get question, option
                            if (questionXml.question[i].answer != null)
                            {
                                for (int j = 0; j < questionXml.question[i].answer.Count(); j++)
                                {
                                    checkHTMLTemp = questionXml.question[i].answer[j].format;
                                    if (questionXml.question[i].answer[j].fraction.ToString().Equals("100"))
                                    {

                                        tempParser = questionXml.question[i].answer[j].text;
                                        tempParser = stringProcess.RemoveHtmlBrTag(tempParser);

                                        if (checkHTML == false)
                                        {
                                            htmlDoc.LoadHtml(tempParser);
                                            tempParser = htmlDoc.DocumentNode.InnerText;
                                        }

                                        rightAnswer = WebUtility.HtmlDecode(tempParser);
                                        rightAnswer = stringProcess.RemoveHtmlTagXML(rightAnswer);

                                        option = new OptionTemp();
                                        if (checkHTMLTemp.Equals("html"))
                                        {
                                            option.OptionContent = "[html]" + rightAnswer;
                                        }
                                        else
                                        {
                                            option.OptionContent = rightAnswer;
                                        }
                                        //rightAnswer = StringProcess.RemoveTag(rightAnswer, @"\n", @"<cbr>");


                                        option.IsCorrect = true;
                                        tempAns.Add(option);
                                        tempParser = "";
                                    }
                                    else
                                    if (questionXml.question[i].answer[j].fraction.ToString().Equals("0"))
                                    {
                                        tempParser = questionXml.question[i].answer[j].text;
                                        tempParser = stringProcess.RemoveHtmlBrTag(tempParser);

                                        if (checkHTML == false)
                                        {
                                            htmlDoc.LoadHtml(tempParser);
                                            tempParser = htmlDoc.DocumentNode.InnerText;
                                        }

                                        wrongAnswer = WebUtility.HtmlDecode(tempParser);
                                        wrongAnswer = stringProcess.RemoveHtmlTagXML(wrongAnswer);

                                        //wrongAnswer = StringProcess.RemoveTag(wrongAnswer, @"\n", @"<cbr>");
                                        option = new OptionTemp();
                                        if (checkHTMLTemp.Equals("html"))
                                        {
                                            option.OptionContent = "[html]" + wrongAnswer;
                                        }
                                        else
                                        {
                                            option.OptionContent = wrongAnswer;
                                        }
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
                                    Status = status,
                                    Code = question.Code,
                                    Category = question.Category,
                                    LearningOutcome = question.LearningOutcome,
                                    LevelName = question.Level,
                                    Image = question.Image,
                                    OptionTemps = tempAns.Select(o => new OptionTemp()
                                    {
                                        OptionContent = o.OptionContent,
                                        IsCorrect = o.IsCorrect
                                    }).ToList()

                                });
                                import.UpdatedDate = DateTime.Now;
                                import.UserId = userId;


                            }
                            int g = 0;
                            string time = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                            string fileName = "XMLFILE-" + user + @"-" + time.ToString() + ".txt";
                            string filePath = "ErrorLog\\";
                            string fullPath = AppDomain.CurrentDomain.BaseDirectory + filePath + fileName;
                            string path = AppDomain.CurrentDomain.BaseDirectory + filePath;
                            if (!File.Exists(fullPath))
                            {
                                var myFile = File.Create(fullPath);
                                myFile.Close();
                                using (StreamWriter tw = new StreamWriter(Path.Combine(path, fileName)))
                                {
                                    if (listQuestionXml != null)
                                    {
                                        foreach (var item in listQuestionXml)
                                        {
                                            countLog++;
                                            tw.WriteLine(countLog + "");
                                            tw.WriteLine("Question: " + item.QuestionContent);
                                            tw.WriteLine("Code: " + item.Code + "\n");
                                            if (item.Options != null)
                                            {
                                                foreach (var itemOp in item.Options)
                                                {
                                                    tw.WriteLine("Option: " + itemOp.OptionContent);
                                                }
                                            }
                                            tw.WriteLine("Error: " + item.Error + "\n");
                                            tw.WriteLine();
                                        }
                                        tw.Close();
                                    }

                                }
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
                    import.Status = (int)Enum.StatusEnum.NotCheck;
                    reader = new StreamReader(questionFile.InputStream, Encoding.UTF8);
                    listQuestion = ulti.StripTagsCharArray(reader, checkCate, checkHTML);
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
                            LearningOutcome = q.LearningOutcome,
                            LevelName = q.Level,
                            OptionTemps = q.Options.Select(o => new OptionTemp()
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect
                            }).ToList(),
                        }).ToList(),
                        UpdatedDate = importTime

                    };


                    int g = 0;
                    string time = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    string fileName = "GIFTFile-" + user + @"-" + time.ToString() + ".txt";
                    string filePath = "ErrorLog\\";
                    string fullPath = AppDomain.CurrentDomain.BaseDirectory + filePath + fileName;
                    string path = AppDomain.CurrentDomain.BaseDirectory + filePath;
                    if (!File.Exists(fullPath))
                    {
                        var myFile = File.Create(fullPath);
                        myFile.Close();
                        using (StreamWriter tw = new StreamWriter(Path.Combine(path, fileName)))
                        {
                            if (listQuestion != null)
                            {
                                foreach (var item in listQuestion)
                                {
                                    g++;
                                    tw.WriteLine(g + "");
                                    tw.WriteLine("Question: " + item.QuestionContent);
                                    tw.WriteLine("Code: " + item.Code + "\n");
                                    if (item.Options != null)
                                    {
                                        foreach (var itemOp in item.Options)
                                        {
                                            tw.WriteLine("Option: " + item.Options + "\n");
                                        }
                                    }
                                    tw.WriteLine("Error: " + item.Error + "\n");
                                    tw.WriteLine();
                                }
                                tw.Close();
                            }                          
                           
                        }    
                    }

                }
                #endregion

                #region process doc

                if (extensionFile.Equals(".doc") || extensionFile.Equals(".docx"))
                {
                    DocUtilities docUltil = new DocUtilities();
                    QuestionTemp quesTmp = new QuestionTemp();
                    reader = new StreamReader(questionFile.InputStream);
                    DateTime importTime = DateTime.Now;
                    listQuestion = docUltil.ParseDoc(questionFile.InputStream,prefix);

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
                            LearningOutcome = prefix + " " +q.LearningOutcome,
                            LevelName = q.Level,
                            Image = q.Image,
                            OptionTemps = q.Options.Select(o => new OptionTemp()
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect
                            }).ToList(),
                        }).ToList(),
                        ImportedDate = DateTime.Now
                    };

                    int g = 0;
                    string time = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    string fileName = "unknownFile-" + user + @"-" + time.ToString() + ".txt";
                    if (extensionFile.Equals(".doc"))
                    {
                        fileName = "DOCFile-" + user + @"-" + time.ToString() + ".txt";
                    }else if (extensionFile.Equals(".docx"))
                    {
                        fileName = "DOCXFile-" + user + @"-" + time.ToString() + ".txt";
                    }
                    string filePath = "ErrorLog\\";
                    string fullPath = AppDomain.CurrentDomain.BaseDirectory + filePath + fileName;
                    string path = AppDomain.CurrentDomain.BaseDirectory + filePath;
                    if (!File.Exists(fullPath))
                    {
                        var myFile = File.Create(fullPath);
                        myFile.Close();
                        using (StreamWriter tw = new StreamWriter(Path.Combine(path, fileName)))
                        {
                            if (listQuestion != null)
                            {
                                foreach (var item in listQuestion)
                                {
                                    g++;
                                    tw.WriteLine(g + "");
                                    tw.WriteLine("Question: " + item.QuestionContent);
                                    tw.WriteLine("Code: " + item.Code + "\n");
                                    if (item.Options != null)
                                    {
                                        foreach (var itemOp in item.Options)
                                        {
                                            tw.WriteLine("Option: " + item.Options + "\n");
                                        }
                                    }
                                    tw.WriteLine("Error: " + item.Error + "\n");
                                    tw.WriteLine();
                                }
                                tw.Close();
                            }

                        }
                    }
                }

                #endregion

                if (import.QuestionTemps.Count() > 0)
                {
                    import.Status = (int)Enum.StatusEnum.NotCheck;
                    import.CourseId = courseId;
                    import.OwnerName = ownerName;
                    //check formats
                    import.QuestionTemps = importService.CheckRule(import.QuestionTemps.ToList());
                    var entity = unitOfWork.Repository<Import>().InsertAndReturn(import);
                    import.TotalQuestion = import.QuestionTemps.Count();
                    unitOfWork.SaveChanges();

                    //log import
                    logService.LogManually("Import", "Question", targetId: entity.Id, controller: "Question",method: "ImportFile", userId: userId);

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
                questionFile.InputStream.Flush();
            }

            return check;
        }
        
        public int GetCountOfListQuestionByLearningOutcomeAndId(int learningOutcomeId, int levelId)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            List<Question> question = questions.Where(q => q.LearningOutcomeId == learningOutcomeId && q.LevelId == levelId).ToList();
            if (question == null)
            {
                return 0;
            }
            return question.Count;

        }

        public int GetMinFreQuencyByLearningOutcome(int learningOutcomeId, int levelId)
        {
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetNoTracking();
            Question question = questions.Where(q => q.LearningOutcomeId == learningOutcomeId && q.LevelId == levelId).OrderBy(q => q.Frequency).Take(1).FirstOrDefault();
            return question != null ? (int)question.Frequency : 0;
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
            }
            else if (categoryId == 0)
            {
                result = result.Where(q => q.CategoryId == null);
            }

            if (learningoutcomeId != null && learningoutcomeId != 0)
            {
                result = result.Where(q => q.LearningOutcomeId == learningoutcomeId);
            }
            else if (learningoutcomeId == 0)
            {
                result = result.Where(q => q.LearningOutcomeId == null);
            }


            if (levelId != null && levelId != 0)
            {
                result = result.Where(q => q.LevelId == levelId);
            }
            else if (levelId == 0)
            {
                result = result.Where(q => q.LevelId == null);
            }

            var list = result.ToList();

            return list.Select(q => new QuestionViewModel
            {
                Id = q.Id,
                Code = q.QuestionCode,
                QuestionContent = WebUtility.HtmlDecode(q.QuestionContent),
                Image = q.Image != null ? q.Image.ToString() : "",
                ImportId = (int)q.ImportId,
                CategoryId = q.CategoryId.HasValue ? q.CategoryId.Value : 0,
                LearningOutcomeId = q.LearningOutcomeId.HasValue ? q.LearningOutcomeId.Value : 0,
                LevelId = q.LevelId.HasValue ? q.LevelId.Value : 0,
                Options = q.Options.ToList().Select(o => new OptionViewModel
                {
                    Id = o.Id,
                    OptionContent = WebUtility.HtmlDecode(o.OptionContent),
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
                        entity.LearningOutcomeId = learningOutcomeId; // fix here
                    }
                    else
                    {
                        entity.LearningOutcomeId = null;
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

        public QuestionHistoryViewModel GetQuestionHistory(int id)
        {
            var examList = new List<ExaminationViewModel>();
            var questionEntity = unitOfWork.Repository<Question>().GetById(id);
            var questionVM = new QuestionViewModel()
            {
                QuestionContent = questionEntity.QuestionContent,
                Image = questionEntity.Image,
                QuestionCode = questionEntity.QuestionCode,
                CourseId = questionEntity.CourseId.Value,
                Options = questionEntity.Options.Select( o => new OptionViewModel
                {
                    IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                    OptionContent = o.OptionContent,
                    Image = o.Image
                }).ToList(),
                Category = (questionEntity.CategoryId.HasValue ? questionEntity.Category.Name : "[None of category]") 
                            + " / " 
                            + (questionEntity.LearningOutcomeId.HasValue ? questionEntity.LearningOutcome.Name : "[None of learning outcome]"),
                LevelId = questionEntity.LevelId.HasValue ? questionEntity.LevelId.Value : 0
            };

            var questionInExams = unitOfWork.Repository<QuestionInExam>().GetAll()
                                    .Where(qe => qe.QuestionReference == id).ToList();
            foreach (var questionInExam in questionInExams)
            {
                var entity = unitOfWork.Repository<Examination>().GetById(questionInExam.PartOfExamination.ExaminationId);
                var exam = new ExaminationViewModel()
                {
                    Id = entity.Id,
                    GeneratedDate = (DateTime)entity.GeneratedDate,
                    //Semester = (int)entity.Semester
                    ExamCode = entity.ExamCode,
                    IsDisable = entity.IsDisable.HasValue && entity.IsDisable.Value
                    
                };
                examList.Add(exam);
            }
            var questionHistory = new QuestionHistoryViewModel()
            {
                Question = questionVM,
                Examination = examList
            };
            questionHistory.Examination = examList.OrderByDescending(q => q.GeneratedDate).ToList();
            return questionHistory;
        }

        public bool InsertQuestionWithTableString(string table, int userId, int courseId)
        {
            var optionCheck = new DocViewModel();
            var optionCheckList = new List<DocViewModel>();
            bool check = false;
            List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();
            List<OptionTemp> optionList = new List<OptionTemp>();
            var optionModel = new OptionTemp();
            var import = new Import();
            table = TrimTags(table);
            XElement parseTable = XElement.Parse(table);
            foreach (XElement eTable in parseTable.Elements("table"))
            {
                QuestionTmpModel questionTmp = new QuestionTmpModel();
                foreach (XElement tbody in eTable.Elements("tbody"))
                {
                    foreach (XElement tr in tbody.Elements("tr"))
                    {
                        var key = tr.Elements("td").ElementAt(0).Value;
                        var value = tr.Elements("td").ElementAt(1).Value;
                        if (key.Contains("QN="))
                        {
                            questionTmp.Code = key.Replace("QN=", "");
                            var contentQ = tr.Elements("td").Elements("p").ToList();
                            for (int i = 1; i < contentQ.Count; i++)
                            {
                                if (contentQ.ElementAt(i).ToString().Contains("base64,"))
                                {
                                    var getImage1 = contentQ.ElementAt(i).ToString().Split(new string[] { "base64," }, StringSplitOptions.None);
                                    var getImage2 = getImage1[1].Split('"');
                                    questionTmp.Image = getImage2[0];
                                }
                                else if (questionTmp.QuestionContent == null &&
                                    !(contentQ.ElementAt(i).ToString().Contains("[file") || contentQ.ElementAt(i).ToString().Equals("")))
                                {
                                    var stringToValue = HttpUtility.HtmlDecode(TrimSpace(contentQ.ElementAt(i).ToString()));
                                    questionTmp.QuestionContent = "[html]" + stringToValue.Replace("<br />", "<cbr>");
                                }
                                else if (!(contentQ.ElementAt(i).ToString().Contains("[file") || contentQ.ElementAt(i).ToString().Equals("")))
                                {
                                    var stringToValue = HttpUtility.HtmlDecode(TrimSpace(contentQ.ElementAt(i).ToString()));
                                    questionTmp.QuestionContent = questionTmp.QuestionContent + "<cbr>" + stringToValue.Replace("<br />", "<cbr>");
                                }
                                //switch (i)
                                //{
                                //    case 1:
                                //        questionTmp.QuestionContent = TrimSpace(contentQ.ElementAt(i).Value);
                                //        break;
                                //    case 3:
                                //        if (contentQ.ElementAt(i).ToString().Contains("png;base64,"))
                                //        {
                                //            var test = contentQ.ElementAt(i).ToString().Split(new string[] { "png;base64," }, StringSplitOptions.None);
                                //            var test1 = test[1].Split('"');
                                //            questionTmp.Image = test1[0];
                                //        }
                                //        break;
                                //}
                            }
                        }
                        else if (key.Contains("."))
                        {
                            optionCheck.Code = key.Replace(".", "");
                            var contentO = tr.Elements("td").Elements("p").ToList();
                            if (!value.Equals(""))
                            {
                                optionModel.IsCorrect = false;
                                for (int i = 1; i < contentO.Count; i++)
                                {
                                    if (contentO.ElementAt(i).ToString().Contains("base64,"))
                                    {
                                        var getImage1 = contentO.ElementAt(i).ToString().Split(new string[] { "base64," }, StringSplitOptions.None);
                                        var getImage2 = getImage1[1].Split('"');
                                        optionModel.Image = getImage2[0];
                                    }
                                    else if (optionModel.OptionContent == null)
                                    {
                                        var stringToValue = HttpUtility.HtmlDecode(TrimSpace(contentO.ElementAt(i).ToString()));
                                        optionModel.OptionContent = stringToValue.Replace("<br/>", "<cbr>");
                                    }
                                    else
                                    {
                                        var stringToValue = HttpUtility.HtmlDecode(TrimSpace(contentO.ElementAt(i).ToString()));
                                        optionModel.OptionContent = optionModel.OptionContent + "<cbr>" + stringToValue.Replace("<br />", "<cbr>");
                                    }
                                }
                            }
                            if (optionModel.OptionContent != null)
                            {
                                optionCheck.Content = optionModel.OptionContent;
                                optionCheckList.Add(optionCheck);
                                optionList.Add(optionModel);
                            }
                            optionModel = new OptionTemp();
                            optionCheck = new DocViewModel();
                        }
                        else if (key.Contains("ANSWER:"))
                        {
                            var trim = value.Replace(" ", "").ToLower();
                            char[] answers = trim.ToCharArray();
                            foreach (var optCheck in optionCheckList)
                            {
                                for (int i = 0; i < answers.Length; i++)
                                {
                                    if (optCheck.Code.Equals(answers[i].ToString()))
                                    {
                                        foreach (var option in optionList)
                                        {
                                            if (option.OptionContent.Equals(optCheck.Content))
                                            {
                                                option.IsCorrect = true;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else if (key.Contains("UNIT:"))
                        {
                            if (!value.Equals(""))
                            {
                                questionTmp.LearningOutcome = "LearningOutcome" + value;
                            }
                        }
                        else if (key.Contains("MARK:"))
                        {
                            if (!value.Equals(""))
                            {
                                switch (value)
                                {
                                    //default:
                                    //    quesModel.Level = "Easy";
                                    //    break;
                                    case "1":
                                        questionTmp.Level = "Easy";
                                        break;
                                    case "2":
                                        questionTmp.Level = "Medium";
                                        break;
                                    case "3":
                                        questionTmp.Level = "Hard";
                                        break;
                                }
                            }
                        }
                    }
                    questionTmp.Options = optionList;
                    listQuestion.Add(questionTmp);
                    questionTmp = new QuestionTmpModel();
                    optionList = new List<OptionTemp>();
                    optionCheckList = new List<DocViewModel>();
                }
            }

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
                    LearningOutcome = q.LearningOutcome,
                    LevelName = q.Level,
                    Image = q.Image,
                    OptionTemps = q.Options.Select(o => new OptionTemp()
                    {
                        OptionContent = o.OptionContent,
                        IsCorrect = o.IsCorrect
                    }).ToList(),
                }).ToList(),
                ImportedDate = DateTime.Now
            };

            if (import.QuestionTemps.Count() > 0)
            {
                import.Status = (int)Enum.StatusEnum.NotCheck;
                import.CourseId = courseId;
                //import.OwnerName = ownerName;
                //check formats
                import.QuestionTemps = importService.CheckRule(import.QuestionTemps.ToList());
                var entity = unitOfWork.Repository<Import>().InsertAndReturn(import);
                import.TotalQuestion = import.QuestionTemps.Count();
                unitOfWork.SaveChanges();

                //log import
                logService.LogManually("Import", "Question", targetId: entity.Id, controller: "Question", method: "ImportFile", userId: userId);

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
            //catch (Exception ex)
            //{
            //    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            //    //Console.WriteLine(ex.Message);
            //}

            return check;
        }

        private string TrimSpace(string trim)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            trim = regex.Replace(trim, " ");
            return trim;
        }

        private string TrimTags(string table)
        {
            table = table.Replace("<st1:country-region>", " ");
            table = table.Replace("</st1:country-region>", " ");
            table = table.Replace("<st1:city>", " ");
            table = table.Replace("</st1:city>", " ");
            table = table.Replace("<st1:place>", " ");
            table = table.Replace("</st1:place>", " ");
            table = table.Replace("<p>&nbsp;</p>", "");
            table = table.Replace("&nbsp;", "");
            return table;
        }
    }
}
