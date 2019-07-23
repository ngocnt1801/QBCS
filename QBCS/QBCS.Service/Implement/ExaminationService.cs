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
using QBCS.Service.Utilities;
using System.Net;
using System.Data.SqlClient;
using System.Data.Entity;

namespace QBCS.Service.Implement
{
    public class ExaminationService : IExaminationService
    {
        private const string EASY = "Easy";
        private const string MEDIUM = "Medium";
        private const string HARD = "Hard";
        private IUnitOfWork unitOfWork;
        private ILevelService levelService;
        private IQuestionService questionService;
        private ILearningOutcomeService learningOutcomeService;
        private ICourseService courseService;
        private ITopicService topicService;
        private ISemesterService semesterService;
        private IPartOfExamService partOfExamService;
        private ILogService logService;
        private IQuestionInExamService questionInExamService;

        public ExaminationService()
        {
            unitOfWork = new UnitOfWork();
            levelService = new LevelService();
            questionService = new QuestionService();
            learningOutcomeService = new LearningOutcomeService();
            topicService = new TopicService();
            courseService = new CourseService();
            partOfExamService = new PartOfExamService();
            semesterService = new SemesterService();
            logService = new LogService();
            questionInExamService = new QuestionInExamService();
        }
        public List<ExaminationViewModel> GetAllExam()
        {
            List<ExaminationViewModel> result = new List<ExaminationViewModel>();
            List<Examination> exams = unitOfWork.Repository<Examination>().GetAll().OrderByDescending(e => e.GeneratedDate).ToList();
            result = exams.Select(e => new ExaminationViewModel
            {
                Id = e.Id,
                ExamCode = e.ExamCode,
                ExamGroup = e.GroupExam,
                IsDisable = (bool)e.IsDisable,
                CourseId = e.CourseId.HasValue ? (int)e.CourseId : 0,
                GeneratedDate = (DateTime)e.GeneratedDate,
                NumberOfEasy = e.NumberOfEasy.HasValue ? (int)e.NumberOfEasy : 0,
                NumberOfMedium = e.NumberOfMedium.HasValue ? (int)e.NumberOfMedium : 0,
                NumberOfHard = e.NumberOfHard.HasValue ? (int)e.NumberOfHard : 0,
                Course = courseService.GetCourseById(e.CourseId.HasValue ? (int)e.CourseId : 0)
            }).ToList();
            return result;
        }
        public List<ExaminationViewModel> GetExamByExamGroup(string groupExam)
        {
            List<ExaminationViewModel> result = new List<ExaminationViewModel>();
            List<Examination> exams = unitOfWork.Repository<Examination>().GetAll().Where(e => e.GroupExam.Equals(groupExam)).ToList();
            result = exams.Select(e => new ExaminationViewModel
            {
                Id = e.Id,
                ExamCode = e.ExamCode,
                ExamGroup = e.GroupExam,
                CourseId = e.CourseId.HasValue ? (int)e.CourseId : 0,
                GeneratedDate = (DateTime)e.GeneratedDate,
                SemesterId = e.SemesterId.HasValue ? (int)e.SemesterId : 0,
                Semester = semesterService.GetById(e.SemesterId.HasValue ? (int)e.SemesterId : 0),
                NumberOfEasy = e.NumberOfEasy.HasValue ? (int)e.NumberOfEasy : 0,
                NumberOfMedium = e.NumberOfMedium.HasValue ? (int)e.NumberOfMedium : 0,
                NumberOfHard = e.NumberOfHard.HasValue ? (int)e.NumberOfHard : 0,
                Course = courseService.GetCourseById(e.CourseId.HasValue ? (int)e.CourseId : 0),
                PartOfExam = partOfExamService.GetPartOfExamByExamId(e.Id)
            }).ToList();
            return result;
        }
        public ExaminationViewModel GetExanById(int examId)
        {
            ExaminationViewModel result = new ExaminationViewModel();
            Examination exam = unitOfWork.Repository<Examination>().GetAll().Where(e => e.Id == examId).FirstOrDefault();
            if (exam != null)
            {
                result = new ExaminationViewModel
                {
                    Id = exam.Id,
                    ExamCode = exam.ExamCode,
                    ExamGroup = exam.GroupExam,
                    IsDisable = (bool)exam.IsDisable,
                    CourseId = exam.CourseId.HasValue ? (int)exam.CourseId : 0,
                    GeneratedDate = (DateTime)exam.GeneratedDate,
                    SemesterId = exam.SemesterId.HasValue ? (int)exam.SemesterId : 0,
                    Semester = semesterService.GetById(exam.SemesterId.HasValue ? (int)exam.SemesterId : 0),
                    NumberOfEasy = exam.NumberOfEasy.HasValue ? (int)exam.NumberOfEasy : 0,
                    NumberOfMedium = exam.NumberOfMedium.HasValue ? (int)exam.NumberOfMedium : 0,
                    NumberOfHard = exam.NumberOfHard.HasValue ? (int)exam.NumberOfHard : 0,
                    Course = courseService.GetCourseById(exam.CourseId.HasValue ? (int)exam.CourseId : 0),
                    PartOfExam = partOfExamService.GetPartOfExamByExamId(exam.Id)
                };
            }
            return result;
        }

        public string GetExamCode()
        {
            string result = "";
            Examination exam = unitOfWork.Repository<Examination>().GetAll().OrderByDescending(e => e.Id).FirstOrDefault();
            if (exam == null || string.IsNullOrEmpty(exam.ExamCode))
            {
                int count = 1;
                result = "EX" + count.ToString("D6");
            }
            else
            {
                string number = exam.ExamCode.Substring(2);
                int count = int.Parse(number);
                count++;
                result = "EX" + count.ToString("D6");
            }
            return result;
        }
        public string GetExamGroup()
        {
            string result = "";
            Examination exam = unitOfWork.Repository<Examination>().GetAll().OrderByDescending(e => e.Id).FirstOrDefault();
            if (exam == null || string.IsNullOrEmpty(exam.GroupExam))
            {
                int count = 1;
                result = "EG" + count.ToString("D6");
            }
            else
            {
                string number = exam.GroupExam.Substring(2);
                int count = int.Parse(number);
                count++;
                result = "EG" + count.ToString("D6");
            }
            return result;
        }
        public void DisableEaxam(int examId)
        {
            Examination exam = unitOfWork.Repository<Examination>().GetById(examId);
            exam.IsDisable = true;
            unitOfWork.Repository<Examination>().Update(exam);
            unitOfWork.SaveChanges();
        }
        public void ResetPriorityAndFrequency(string groupExam)
        {
            List<Examination> examsByGroup = unitOfWork.Repository<Examination>().GetAll()
                .Where(e => e.GroupExam.Equals(groupExam))
                .OrderByDescending(e => e.Id).ToList();
            foreach (Examination exam in examsByGroup)
            {
                List<PartOfExamViewModel> partOfExam = partOfExamService.GetPartOfExamByExamId(exam.Id);
                foreach (var part in partOfExam)
                {
                    foreach (var ques in part.Question)
                    {
                        Question question = unitOfWork.Repository<Question>().GetById(ques.QuestionReference);
                        if (question != null)
                        {
                            question.Frequency = ques.Frequency;
                            question.Priority = ques.Priority;
                            unitOfWork.Repository<Question>().Update(question);
                            unitOfWork.SaveChanges();
                        }
                    }
                }
                exam.IsDisable = true;
                unitOfWork.Repository<Examination>().Update(exam);
                unitOfWork.SaveChanges();
            }
        }
        public GenerateExamViewModel SaveQuestionsToExam(List<string> questionCode, int courseId, int semeterId)
        {
            int questionEasy = 0;
            int questionMedium = 0;
            int questionHard = 0;
            int idOfEasyLevel = levelService.GetIdByName(EASY);
            int idOfMediumLevel = levelService.GetIdByName(MEDIUM);
            int idOfHardLevel = levelService.GetIdByName(HARD);
            string groupExam = GetExamGroup();
            string examCode = GetExamCode();
            Examination exam = new Examination()
            {
                ExamCode = examCode,
                GroupExam = groupExam,
                CourseId = courseId,
                SemesterId = semeterId,
                IsDisable = false,
                GeneratedDate = DateTime.Now
            };
            unitOfWork.Repository<Examination>().Insert(exam);
            unitOfWork.SaveChanges();
            foreach (var quesCode in questionCode)
            {
                QuestionViewModel questionViewModel = questionService.GetQuestionByQuestionCode(quesCode);
                if (questionViewModel.LevelId == idOfEasyLevel)
                {
                    questionEasy++;
                }
                else if (questionViewModel.LevelId == idOfMediumLevel)
                {
                    questionMedium++;
                }
                else
                {
                    questionHard++;
                }
                PartOfExamination part;
                if (exam.PartOfExaminations.Count == 0 || exam.PartOfExaminations.Where(p => p.LearningOutcomeId == questionViewModel.LearningOutcomeId).FirstOrDefault() == null)
                {
                    part = new PartOfExamination()
                    {
                        LearningOutcomeId = questionViewModel.LearningOutcomeId,
                        ExaminationId = exam.Id,
                        NumberOfQuestion = 0
                    };
                    unitOfWork.Repository<PartOfExamination>().Insert(part);
                    unitOfWork.SaveChanges();

                }
                else
                {
                    part = exam.PartOfExaminations.Where(p => p.LearningOutcomeId == questionViewModel.LearningOutcomeId).FirstOrDefault();
                }
                QuestionInExam question = new QuestionInExam()
                {
                    QuestionContent = questionViewModel.QuestionContent,
                    PartId = part.Id,
                    QuestionReference = questionViewModel.Id,
                    Priority = questionViewModel.Priority,
                    Frequency = questionViewModel.Frequency,
                    Image = questionViewModel.Image,
                    QuestionCode = questionViewModel.QuestionCode,
                    LevelId = questionViewModel.LevelId,
                    CategoryId = questionViewModel.CategoryId,
                    IsDisable = false,
                    OptionInExams = questionViewModel.Options.Select(o => new OptionInExam()
                    {
                        IsCorrect = o.IsCorrect,
                        OptionContent = o.OptionContent,
                        Image = o.Image
                    }).ToList()
                };
                part.NumberOfQuestion = part.NumberOfQuestion + 1;
                unitOfWork.Repository<PartOfExamination>().Update(part);
                unitOfWork.Repository<QuestionInExam>().Insert(question);
                unitOfWork.SaveChanges();
            }//end for
            //update Frequency and Priority
            foreach (var part in exam.PartOfExaminations)
            {
                //update easy level
                if (part.QuestionInExams.Where(q => q.LevelId == idOfEasyLevel) != null)
                {
                    List<Question> questionByLevelAndLO = unitOfWork.Repository<Question>().GetAll().Where(q => q.LevelId == idOfEasyLevel && q.LearningOutcomeId == part.LearningOutcomeId).ToList();
                    foreach (var questionByLevel in questionByLevelAndLO)
                    {
                        if (part.QuestionInExams.Any(q => q.QuestionReference == questionByLevel.Id))
                        {
                            questionByLevel.Priority = 0;
                            questionByLevel.Frequency = questionByLevel.Frequency + 1;
                        }
                        else
                        {
                            questionByLevel.Priority = questionByLevel.Priority + 1;
                        }
                        unitOfWork.Repository<Question>().Update(questionByLevel);
                    }
                }
                //update medium question
                if (part.QuestionInExams.Where(q => q.LevelId == idOfMediumLevel) != null)
                {
                    List<Question> questionByLevelAndLO = unitOfWork.Repository<Question>().GetAll().Where(q => q.LevelId == idOfMediumLevel && q.LearningOutcomeId == part.LearningOutcomeId).ToList();
                    foreach (var questionByLevel in questionByLevelAndLO)
                    {
                        if (part.QuestionInExams.Any(q => q.QuestionReference == questionByLevel.Id))
                        {
                            questionByLevel.Priority = 0;
                            questionByLevel.Frequency = questionByLevel.Frequency + 1;
                        }
                        else
                        {
                            questionByLevel.Priority = questionByLevel.Priority + 1;
                        }
                        unitOfWork.Repository<Question>().Update(questionByLevel);
                    }
                }

                //update hard question
                if (part.QuestionInExams.Where(q => q.LevelId == idOfHardLevel) != null)
                {
                    List<Question> questionByLevelAndLO = unitOfWork.Repository<Question>().GetAll().Where(q => q.LevelId == idOfHardLevel && q.LearningOutcomeId == part.LearningOutcomeId).ToList();
                    foreach (var questionByLevel in questionByLevelAndLO)
                    {
                        if (part.QuestionInExams.Any(q => q.QuestionReference == questionByLevel.Id))
                        {
                            questionByLevel.Priority = 0;
                            questionByLevel.Frequency = questionByLevel.Frequency + 1;
                        }
                        else
                        {
                            questionByLevel.Priority = questionByLevel.Priority + 1;
                        }
                        unitOfWork.Repository<Question>().Update(questionByLevel);
                    }
                }
            }
            // save question list
            unitOfWork.SaveChanges();
            exam.NumberOfEasy = questionEasy;
            exam.NumberOfMedium = questionMedium;
            exam.NumberOfHard = questionHard;
            unitOfWork.Repository<Examination>().Update(exam);
            unitOfWork.SaveChanges();
            int totalQuestion = questionEasy + questionMedium + questionHard;
            int percentQuestionEasy = questionEasy / totalQuestion;
            int percentQuestionMedium = questionMedium / totalQuestion;
            int percentQuestionHard = 100 - percentQuestionEasy - percentQuestionMedium;
            GenerateExamViewModel result = new GenerateExamViewModel()
            {
                ExamId = exam.Id,
                EasyQuestion = questionEasy,
                MediumQuestion = questionMedium,
                HardQuestion = questionHard,
                EasyQuestionGenerrate = questionEasy,
                MediumQuestionGenerrate = questionMedium,
                HardQuestionGenerrate = questionHard,
                GroupExam = exam.GroupExam,
                TotalQuestion = totalQuestion,
                EasyPercent = percentQuestionEasy,
                MediumPercent = percentQuestionMedium,
                HardPercent = percentQuestionHard,
                TotalQuestionGenerrate = totalQuestion,
                TotalExam = 1,
                IsEnough = true
            };
            result.CalculateGrade();
            return result;
        }

        public GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam, string fullname = "", string usercode = "")
        {
            exam.IsEnough = true;
            int courseId = exam.CourseId;
            CourseViewModel course = courseService.GetCourseById(courseId);
            exam.TotalQuestion = course.DefaultNumberOfQuestion;
            if (exam.FlagPercent.Equals("grade"))
            {
                exam.EasyPercent = exam.OrdinaryGrade;
                exam.MediumPercent = exam.GoodGrade - exam.OrdinaryGrade;
                exam.HardPercent = 100 - exam.EasyPercent - exam.MediumPercent;
            }
            int questionEasy = (int)Math.Ceiling((exam.TotalQuestion * exam.EasyPercent * 1.0) / 100);
            int questionMedium = (int)Math.Ceiling((exam.TotalQuestion * exam.MediumPercent * 1.0) / 100);
            int questionHard = exam.TotalQuestion - questionEasy - questionMedium;
            exam.EasyQuestion = questionEasy;
            exam.MediumQuestion = questionMedium;
            exam.HardQuestion = questionHard;
            int totalEasyQuestionInTopicCategory = 0;
            int totalMediumQuestionInTopicCategory = 0;
            int totalHardQuestionInTopicCategory = 0;
            List<LearingOutcomeInExamination> topics = new List<LearingOutcomeInExamination>();
            foreach (string topic in exam.Topic)
            {
                int totalEasyQuestionInTopic = 0;
                int totalMediumQuestionInTopic = 0;
                int totalHardQuestionInTopic = 0;
                int id = int.Parse(topic.Substring(3));
                if (topic.Contains("LO_"))
                {
                    int idOfLevel = levelService.GetIdByName(EASY);
                    totalEasyQuestionInTopic = questionService.GetCountOfListQuestionByLearningOutcomeAndId(id, idOfLevel);
                    totalEasyQuestionInTopicCategory += totalEasyQuestionInTopic;

                    idOfLevel = levelService.GetIdByName(MEDIUM);
                    totalMediumQuestionInTopic = questionService.GetCountOfListQuestionByLearningOutcomeAndId(id, idOfLevel);
                    totalMediumQuestionInTopicCategory += totalMediumQuestionInTopic;

                    idOfLevel = levelService.GetIdByName(HARD);
                    totalHardQuestionInTopic = questionService.GetCountOfListQuestionByLearningOutcomeAndId(id, idOfLevel);
                    totalHardQuestionInTopicCategory += totalHardQuestionInTopic;
                }
                LearingOutcomeInExamination learningOutcomeInExam = new LearingOutcomeInExamination()
                {
                    Id = id,
                    TotalEasyQuestionInTopic = totalEasyQuestionInTopic,
                    TotalMediumQuestionInTopic = totalMediumQuestionInTopic,
                    TotalHardQuestionInTopic = totalHardQuestionInTopic
                };
                topics.Add(learningOutcomeInExam);
            }
            if (questionEasy > totalEasyQuestionInTopicCategory)
            {
                questionEasy = totalEasyQuestionInTopicCategory;
                exam.EasyQuestionGenerrate = totalEasyQuestionInTopicCategory;
                exam.IsEnough = false;
            }
            else
            {
                exam.EasyQuestionGenerrate = questionEasy;
            }
            if (questionHard > totalHardQuestionInTopicCategory)
            {
                questionHard = totalHardQuestionInTopicCategory;
                exam.HardQuestionGenerrate = totalHardQuestionInTopicCategory;
                exam.IsEnough = false;
            }
            else
            {
                exam.HardQuestionGenerrate = questionHard;
            }
            if (questionMedium > totalMediumQuestionInTopicCategory)
            {
                questionMedium = totalMediumQuestionInTopicCategory;
                exam.MediumQuestionGenerrate = totalMediumQuestionInTopicCategory;
                exam.IsEnough = false;
            }
            else
            {
                exam.MediumQuestionGenerrate = questionMedium;
            }
            exam.TotalQuestionGenerrate = questionEasy + questionHard + questionMedium;
            if (exam.IsEnough == false)
            {
                return exam;
            }
            if (course.Syllabus.Count == 0)
            {
                while (questionEasy != 0 || questionMedium != 0 || questionHard != 0)
                {
                    for (int i = 0; i < topics.Count; i++)
                    {
                        if (questionEasy != 0)
                        {
                            if (topics[i].EasyQuestion == topics[i].TotalEasyQuestionInTopic)
                            {
                                continue;
                            }
                            else
                            {
                                topics[i].EasyQuestion = topics[i].EasyQuestion + 1;
                                questionEasy--;
                            }
                        }
                        else if (questionMedium != 0)
                        {
                            if (topics[i].MediumQuestion == topics[i].TotalMediumQuestionInTopic)
                            {
                                continue;
                            }
                            else
                            {
                                topics[i].MediumQuestion = topics[i].MediumQuestion + 1;
                                questionMedium--;
                            }

                        }
                        else if (questionHard != 0)
                        {
                            if (topics[i].HardQuestion == topics[i].TotalHardQuestionInTopic)
                            {
                                continue;
                            }
                            else
                            {
                                topics[i].HardQuestion = topics[i].HardQuestion + 1;
                                questionHard--;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                List<SyllabusPartialViewModel> syllabus = course.Syllabus.Select(s => new SyllabusPartialViewModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    AmountQuestion = s.AmountQuestion,
                    LearingOutcomes = s.LearingOutcomes
                }).ToList();
                foreach (var syl in syllabus)
                {
                    if (syl.AmountQuestion == 0)
                    {
                        continue;
                    }
                    int totalEasyQuestionInBank = 0;
                    int totalMediumQuestionInBank = 0;
                    int totalHardQuestionInBank = 0;
                    List<LearingOutcomeInExamination> temp = new List<LearingOutcomeInExamination>();
                    foreach (var topic in topics)
                    {
                        if (syl.LearingOutcomes.Any(t => t.Id == topic.Id))
                        {
                            temp.Add(topic);
                            totalEasyQuestionInBank += topic.TotalEasyQuestionInTopic;
                            totalHardQuestionInBank += topic.TotalHardQuestionInTopic;
                            totalMediumQuestionInBank += topic.TotalMediumQuestionInTopic;
                        }
                    }
                    if (temp.Count == 0)
                    {
                        continue;
                    }
                    int questionEasyInSyllabus = (int)Math.Ceiling((syl.AmountQuestion * exam.EasyPercent * 1.0) / 100);
                    int questionMediumInSyllabus = (int)Math.Ceiling((syl.AmountQuestion * exam.MediumPercent * 1.0) / 100);
                    int questionHardInSyllabus = syl.AmountQuestion - questionEasyInSyllabus - questionMediumInSyllabus;
                    if (totalEasyQuestionInBank < questionEasyInSyllabus)
                    {
                        exam.IsEnough = false;
                        exam.EasyQuestionGenerrate = totalEasyQuestionInBank;
                        exam.EasyQuestion = questionEasyInSyllabus;
                    }
                    if (totalMediumQuestionInBank < questionMediumInSyllabus)
                    {
                        exam.IsEnough = false;
                        exam.MediumQuestionGenerrate = totalMediumQuestionInBank;
                        exam.MediumQuestion = questionMediumInSyllabus;
                    }
                    if (totalHardQuestionInBank < questionHardInSyllabus)
                    {
                        exam.IsEnough = false;
                        exam.HardQuestionGenerrate = totalMediumQuestionInBank;
                        exam.HardQuestion = questionHardInSyllabus;
                    }

                    if (exam.IsEnough == false)
                    {
                        return exam;
                    }
                    else
                    {
                        while (questionEasyInSyllabus != 0 || questionMediumInSyllabus != 0 || questionHardInSyllabus != 0)
                        {
                            for (int i = 0; i < temp.Count; i++)
                            {
                                if (questionEasyInSyllabus != 0)
                                {
                                    if (temp[i].EasyQuestion == temp[i].TotalEasyQuestionInTopic)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        temp[i].EasyQuestion = temp[i].EasyQuestion + 1;
                                        questionEasyInSyllabus--;
                                    }
                                }
                                else if (questionMediumInSyllabus != 0)
                                {
                                    if (temp[i].MediumQuestion == temp[i].TotalMediumQuestionInTopic)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        temp[i].MediumQuestion = temp[i].MediumQuestion + 1;
                                        questionMediumInSyllabus--;
                                    }

                                }
                                else if (questionHardInSyllabus != 0)
                                {
                                    if (temp[i].HardQuestion == temp[i].TotalHardQuestionInTopic)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        temp[i].HardQuestion = temp[i].HardQuestion + 1;
                                        questionHardInSyllabus--;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }// end while

                        foreach (var tmp in temp)
                        {
                            topics[topics.FindIndex(t => t.Id == tmp.Id)] = tmp;
                        }
                    }
                }
            }
            string examGroup = GetExamGroup();
            for (int i = 0; i < exam.TotalExam; i++)
            {

                Examination examination = new Examination()
                {
                    CourseId = courseId,
                    IsDisable = false,
                    NumberOfHard = exam.HardQuestion,
                    NumberOfEasy = exam.EasyQuestion,
                    NumberOfMedium = exam.MediumQuestion,
                    GeneratedDate = DateTime.Now,
                    ExamCode = GetExamCode(),
                    GroupExam = examGroup
                };
                if (exam.Semeter != 0)
                {
                    examination.SemesterId = exam.Semeter;
                }
                unitOfWork.Repository<Examination>().Insert(examination);
                unitOfWork.SaveChanges();

                //log generate exam
                logService.LogManually("Generate", "Examination", examination.Id, fullname: fullname, usercode: usercode, controller: "Examination", method: "GenerateExaminaton");


                foreach (var topic in topics)
                {
                    List<QuestionViewModel> questionInPartOfExam;
                    PartOfExamination partOfExam;
                    questionInPartOfExam = GeneratePartOfExamByLearningOutcome(topic, exam.CategoryId);
                    partOfExam = new PartOfExamination()
                    {
                        LearningOutcomeId = topic.Id,
                        NumberOfQuestion = topic.EasyQuestion + topic.HardQuestion + topic.MediumQuestion,
                        ExaminationId = examination.Id
                    };
                    unitOfWork.Repository<PartOfExamination>().Insert(partOfExam);
                    unitOfWork.SaveChanges();
                    foreach (var ques in questionInPartOfExam)
                    {
                        QuestionInExam question = new QuestionInExam()
                        {
                            QuestionContent = ques.QuestionContent,
                            PartId = partOfExam.Id,
                            QuestionReference = ques.Id,
                            Priority = ques.Priority,
                            Frequency = ques.Frequency,
                            Image = ques.Image,
                            QuestionCode = ques.QuestionCode,
                            LevelId = ques.LevelId,
                            CategoryId = ques.CategoryId,
                            IsDisable = false,
                            OptionInExams = ques.Options.Select(o => new OptionInExam()
                            {
                                IsCorrect = o.IsCorrect,
                                OptionContent = o.OptionContent,
                                Image = o.Image
                            }).ToList()
                        };
                        unitOfWork.Repository<QuestionInExam>().Insert(question);
                    }
                    unitOfWork.SaveChanges();
                }
            }



            exam.GroupExam = examGroup;
            exam.CalculateGrade();
            return exam;
        }
        private List<QuestionViewModel> GeneratePartOfExamByLearningOutcome(LearingOutcomeInExamination learingOutcomeInExam, int categoryId)
        {
            int idOfLevel = levelService.GetIdByName(EASY);
            List<QuestionViewModel> questionEasy = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, learingOutcomeInExam.EasyQuestion, idOfLevel);
            idOfLevel = levelService.GetIdByName(MEDIUM);
            List<QuestionViewModel> questionMedium = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, learingOutcomeInExam.MediumQuestion, idOfLevel);
            idOfLevel = levelService.GetIdByName(HARD);
            List<QuestionViewModel> questionHard = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, learingOutcomeInExam.HardQuestion, idOfLevel);
            List<QuestionViewModel> result = questionEasy.Concat(questionMedium).Concat(questionHard).ToList();
            return result;
        }
        private List<QuestionViewModel> GeneratePartOfExamByLearningOutcomeAndLevel(int learningOutcomeId, int numberOfQuestion, int idOfLevel)
        {
            List<QuestionViewModel> resultTmp = new List<QuestionViewModel>();
            List<QuestionViewModel> result = new List<QuestionViewModel>();
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            while (result.Count < numberOfQuestion)
            {
                if (result.Count == numberOfQuestion)
                {
                    break;
                }
                int minFrequency = questionService.GetMinFreQuencyByLearningOutcome(learningOutcomeId, idOfLevel);
                List<Question> questionsByLevelAndLearningOutcome = questions.Where(q => q.LevelId == idOfLevel && q.LearningOutcomeId == learningOutcomeId).ToList();
                List<QuestionViewModel> questionViewModelRemoveRecent = questionsByLevelAndLearningOutcome.Where(q => q.Frequency == minFrequency && q.Priority != 0).Select(c => new QuestionViewModel
                {
                    Frequency = c.Frequency.HasValue ? (int)c.Frequency : 0,
                    Id = c.Id,
                    LevelId = c.LevelId.HasValue ? (int)c.LevelId : 0,
                    LearningOutcomeId = c.LearningOutcomeId.HasValue ? (int)c.LearningOutcomeId : 0,
                    Priority = c.Priority.HasValue ? (int)c.Priority : 0,
                    QuestionContent = c.QuestionContent,
                    Image = c.Image,
                    QuestionCode = c.QuestionCode,
                    CategoryId = c.CategoryId.HasValue ? (int)c.CategoryId : 0,
                    Options = c.Options.Select(d => new OptionViewModel
                    {
                        Id = d.Id,
                        OptionContent = d.OptionContent,
                        IsCorrect = (bool)d.IsCorrect,
                        Image = d.Image
                    }).ToList()
                }).ToList();
                var listQuestionRemoveRecent = questionsByLevelAndLearningOutcome
                                                                    .Where(q => q.Frequency == minFrequency && q.Priority != 0)
                                                                    .OrderBy(q => q.Priority)
                                                                    .GroupBy(q => q.Priority)
                                                                    .Select((grp => grp.ToList()))
                                                                    .ToList();
                while (questionViewModelRemoveRecent.Count != 0)
                {
                    for (int i = 0; i < listQuestionRemoveRecent.Count; i++)
                    {
                        if (listQuestionRemoveRecent[i].Count != 0)
                        {
                            Random r = new Random();
                            int randomIndex = r.Next(0, (listQuestionRemoveRecent[i].Count - 1));
                            var question = listQuestionRemoveRecent[i][randomIndex];
                            listQuestionRemoveRecent[i].RemoveAt(randomIndex);
                            QuestionViewModel questionViewModel = questionViewModelRemoveRecent.Where(q => q.Id == question.Id).FirstOrDefault();
                            if (!result.Any(e => e.Id == questionViewModel.Id))
                            {
                                result.Add(questionViewModel);
                            }
                            questionViewModelRemoveRecent.Remove(questionViewModel);
                        }
                        if (result.Count == numberOfQuestion)
                        {
                            break;
                        }
                    }
                    if (result.Count == numberOfQuestion)
                    {
                        break;
                    }
                }
                foreach (Question questionEntity in questionsByLevelAndLearningOutcome)
                {
                    questionEntity.Priority = questionEntity.Priority + 1;
                    unitOfWork.Repository<Question>().Update(questionEntity);
                }
                //object[] xparams = { new SqlParameter("@LevelId", idOfLevel),
                //                    new SqlParameter("@LearningOutcomeId", learningOutcomeId)};
                //unitOfWork.GetContext().Database.ExecuteSqlCommand("EXEC UpdatePriorityIncrease @LevelId, @LearningOutcomeId", xparams);
                unitOfWork.SaveChanges();
                foreach (QuestionViewModel ques in result)
                {
                    if (!resultTmp.Any(tmp => tmp.Id == ques.Id))
                    {
                        Question questionEntity = questionsByLevelAndLearningOutcome.Where(q => q.Id == ques.Id).FirstOrDefault();
                        questionEntity.Frequency = questionEntity.Frequency + 1;
                        questionEntity.Priority = 0;
                        unitOfWork.Repository<Question>().Update(questionEntity);
                        //unitOfWork.SaveChanges();
                        resultTmp.Add(ques);
                    }
                    else
                    {
                        Question questionEntity = questionsByLevelAndLearningOutcome.Where(q => q.Id == ques.Id).FirstOrDefault();
                        questionEntity.Priority = 0;
                        unitOfWork.Repository<Question>().Update(questionEntity);
                        //unitOfWork.SaveChanges();
                    }

                }
                unitOfWork.SaveChanges();
                if (result.Count == numberOfQuestion)
                {
                    break;
                }
            }
            return result;
        }

        public List<QuestionInExamViewModel> GetExaminationHistoryQuestionsInCourse(int courseId)
        {
            var listQuestion = unitOfWork.Repository<Question>().GetAll()
                                        .Where(q => q.CourseId == courseId)
                                        .ToList()
                                        .Select(q => new QuestionInExamViewModel()
                                        {
                                            Id = q.Id,
                                            Frequency = q.Frequency.Value,
                                            Priority = q.Priority.Value,
                                            QuestionContent = WebUtility.HtmlDecode(q.QuestionContent),
                                            LevelId = q.LevelId.HasValue ? q.LevelId.Value : 0,
                                            LearningOutcomeName = q.LearningOutcomeId.HasValue ? q.LearningOutcome.Name : "",
                                            Image = q.Image,
                                            QuestionCode = q.QuestionCode,
                                            Options = q.Options.ToList().Select(o => new OptionViewModel
                                            {
                                                Id = o.Id,
                                                Image = o.Image,
                                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                                                OptionContent = WebUtility.HtmlDecode(o.OptionContent)
                                            }).ToList()
                                        })
                                        .OrderBy(q => q.Priority)
                                        .ThenByDescending(q => q.Frequency)
                                        .ToList();


            return listQuestion;

        }

        public string ReplaceQuestionInExam(int questionId, string fullname = "", string usercode = "")
        {
            QuestionInExamViewModel questionInExam = questionInExamService.GetQuestionInExamById(questionId);
            ExaminationViewModel exam = GetExanById(questionInExam.PartOfExam.ExaminationId);
            QuestionInExam oldQuestion = unitOfWork.Repository<QuestionInExam>().GetById(questionInExam.Id);
            int learningOutcomeId = questionInExam.PartOfExam.LearningOutcomeId;
            int levelId = questionInExam.LevelId;
            int countQuestionInBank = questionService.GetCountOfListQuestionByLearningOutcomeAndId(learningOutcomeId, levelId);
            int countQuestionInExam = questionInExamService.GetCountByLearningOutcome(learningOutcomeId, levelId);
            if (countQuestionInBank == countQuestionInExam)
            {
                foreach (PartOfExamViewModel part in exam.PartOfExam)
                {
                    countQuestionInBank = questionService.GetCountOfListQuestionByLearningOutcomeAndId(part.LearningOutcomeId, levelId);
                    countQuestionInExam = questionInExamService.GetCountByLearningOutcome(part.LearningOutcomeId, levelId);
                    if (countQuestionInExam < countQuestionInBank)
                    {
                        learningOutcomeId = part.LearningOutcomeId;
                    }
                }
            }
            oldQuestion.IsDisable = true;
            List<QuestionViewModel> newQuestion = GeneratePartOfExamByLearningOutcomeAndLevel(learningOutcomeId, 1, levelId);
            foreach (var ques in newQuestion)
            {
                QuestionInExam question = new QuestionInExam()
                {
                    QuestionContent = ques.QuestionContent,
                    PartId = questionInExam.PartId,
                    QuestionReference = ques.Id,
                    Priority = ques.Priority,
                    Frequency = ques.Frequency,
                    Image = ques.Image,
                    QuestionCode = ques.QuestionCode,
                    LevelId = ques.LevelId,
                    CategoryId = ques.CategoryId,
                    IsDisable = false,
                    OptionInExams = ques.Options.Select(o => new OptionInExam()
                    {
                        IsCorrect = o.IsCorrect,
                        OptionContent = o.OptionContent,
                        Image = o.Image
                    }).ToList()
                };
                unitOfWork.Repository<QuestionInExam>().Insert(question);
            }
            unitOfWork.SaveChanges();



            //log delete and replace question exam
            logService.LogManually("Delete And Replace Question", "Examination", exam.Id, fullname: fullname, usercode: usercode, controller: "Examination", method: "DeleteQuestionInExam");

            return exam.ExamGroup;
        }
    }
}
