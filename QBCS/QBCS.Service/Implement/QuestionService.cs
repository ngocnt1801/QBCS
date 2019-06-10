using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace QBCS.Service.Implement
{
    public class QuestionService : IQuestionService
    {
        private IUnitOfWork unitOfWork;

        public QuestionService()
        {
            unitOfWork = new UnitOfWork();
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
            questionById.LearningOutcomeId = question.LearningOutcomeId;
            questionById.TopicId = question.TopicId;

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

        public bool InsertQuestion(HttpPostedFileBase questionFile, int userId, int courseId)
        {
            bool check = false;
            StreamReader reader = null;
            List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();
            var import = new Import();
            try
            {
                 
                string extensionFile = Path.GetExtension(questionFile.FileName);
                

                if (extensionFile.Equals(".xml"))
                {
                    List<QuestionTmpModel> listQuestionXml = new List<QuestionTmpModel>();
                    XmlSerializer xmlSer = new XmlSerializer(typeof(quiz));

                    reader = new StreamReader(questionFile.InputStream);
                    quiz questionXml = (quiz)xmlSer.Deserialize(reader);
                   // List<string> tempQues = new List<string>();
                    List<OptionTemp> tempAns = new List<OptionTemp>();
                   // List<string> tempAnsWrong = new List<string>();
                   // List<string> tempAnsRight = new List<string>();
                    QuestionTmpModel question = new QuestionTmpModel();
                    OptionTemp option = new OptionTemp();

                    for (int i = 0; i < questionXml.question.Count(); i++)
                    {
                        //string jsonAnswerRight = null;
                        //string jsonAnswerWrong = null;
                        string questionContent = null;
                        //string jsonQuestion = null;
                        string rightAnswer = null;
                        string wrongAnswer = null;
                        if (questionXml.question[i].questiontext != null)
                        {
                            //tempQues = new List<string>();

                            questionContent = questionXml.question[i].questiontext.text;
                            //tempQues.Add(questionContent);
                            //jsonQuestion = JsonConvert.SerializeObject(tempQues);
                            question.QuestionContent = questionContent;
                            question.Code = questionXml.question[i].name.text.ToString();
                            if (questionXml.question[i].answer != null)
                            {
                                for (int j = 0; j < questionXml.question[i].answer.Count(); j++)
                                {
                                    //tempAns = new List<OptionTemp>();
                                    if (questionXml.question[i].answer[j].fraction.ToString().Equals("100"))
                                    {
                                        //tempAnsRight = new List<string>();
                                        rightAnswer = questionXml.question[i].answer[j].text;
                                        //tempAnsRight.Add(rightAnswer);
                                        //jsonAnswerRight = JsonConvert.SerializeObject(tempAnsRight);
                                        option = new OptionTemp();
                                        option.OptionContent = rightAnswer;
                                        option.IsCorrect = true;
                                        tempAns.Add(option);
                                        //jsonAnswerRight = JsonConvert.SerializeObject(tempAnsRight);
                                        //question.OptionsContent = jsonAnswerRight;
                                    }
                                    else
                                    if (questionXml.question[i].answer[j].fraction.ToString().Equals("0"))
                                    {
                                        //tempAnsWrong = new List<string>();
                                        wrongAnswer = questionXml.question[i].answer[j].text;
                                        //tempAnsWrong.Add(wrongAnswer);
                                        //jsonAnswerWrong = JsonConvert.SerializeObject(tempAnsWrong);
                                        option = new OptionTemp();
                                        option.OptionContent = wrongAnswer;
                                        option.IsCorrect = false;
                                        tempAns.Add(option);

                                        //option.OptionContent = jsonAnswerWrong;

                                    }

                                }
                            }
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
                                    Status = 1,
                                    Code = question.Code,
                                    OptionTemps = tempAns.Select(o => new OptionTemp()
                                    {
                                        OptionContent = o.OptionContent,
                                        IsCorrect = o.IsCorrect
                                    }).ToList()
                                   
                                });
                                import.ImportedDate = DateTime.Now;
                                import.UserId = userId;
                               

                                //{

                                //    UserId = userId,
                                //    QuestionTemps = listQuestionXml.Select(q => new QuestionTemp()
                                //    {
                                //        QuestionContent = q.QuestionContent,
                                //        // OptionsContent = q.OptionsContent,
                                //        OptionTemps = tempAns.Select(a => new OptionTemp()
                                //        {
                                //            OptionContent = a.OptionContent,
                                //            IsCorrect = a.IsCorrect
                                //        }).ToList(),
                                //        Status = 1
                                //    }).ToList(),
                                //    ImportedDate = importTime,
                                //    CourseId = courseId,
                                //    TotalQuestion = listQuestionXml.Count(),
                                //    Status = 1,

                                //};
                            }
                            listQuestionXml = new List<QuestionTmpModel>();
                            question = new QuestionTmpModel();
                            tempAns = new List<OptionTemp>();
                        }                       
                    }
                    //if (import.QuestionTemps.Count() > 0)
                    //{
                    //    unitOfWork.Repository<Import>().Insert(import);
                    //    unitOfWork.SaveChanges();
                    //    check = true;
                    //}

                    //reader.Close();
                }
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
                        QuestionTemps = listQuestion.Select(q => new QuestionTemp()
                        {
                            QuestionContent = q.QuestionContent,
                            Code = q.Code,
                            OptionTemps = q.Options.Select (o => new OptionTemp()
                            {
                                OptionContent = o.OptionContent,
                                IsCorrect = o.IsCorrect
                            }).ToList(),
                        }).ToList(),
                        ImportedDate = importTime

                    };
                   
                }
                if (import.QuestionTemps.Count() > 0)
                {
                    unitOfWork.Repository<Import>().Insert(import);
                    unitOfWork.SaveChanges();
                    check = true;
                }
                else
                {
                    // return user have to import file
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader.Close();
            }

            return check;
        }
    }

}
