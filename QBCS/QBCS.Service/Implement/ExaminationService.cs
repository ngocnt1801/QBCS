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

namespace QBCS.Service.Implement
{
    public class ExaminationService : IExaminationService
    {
        private const double ORDINARY_STUDENT_EASY_PERCENT = 1;
        private const double ORDINARY_STUDENT_MEDIUM_PERCENT = 0.3;
        private const double ORDINARY_STUDENT_HARD_PERCENT = 0;

        private const double GOOD_STUDENT_EASY_PERCENT = 1;
        private const double GOOD_STUDENT_MEDIUM_PERCENT = 0.7;
        private const double GOOD_STUDENT_HARD_PERCENT = 0.3;

        private const double EXCELLENT_STUDENT_EASY_PERCENT = 1;
        private const double EXCELLENT_STUDENT_MEDIUM_PERCENT = 1;
        private const double EXCELLENT_STUDENT_HARD_PERCENT = 0.6;

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
        }
        public List<ExaminationViewModel> GetAllExam()
        {
            List<ExaminationViewModel> result = new List<ExaminationViewModel>();
            List<Examination> exams = unitOfWork.Repository<Examination>().GetAll().OrderByDescending(e => e.GeneratedDate).ToList();
            result = exams.Select(e => new ExaminationViewModel
            {
                Id = e.Id,
                ExamCode = e.ExamCode,
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
                CourseId = e.CourseId.HasValue ? (int)e.CourseId : 0,
                GeneratedDate = (DateTime)e.GeneratedDate,
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
        public GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam)
        {
            exam.IsEnough = true;
            int courseId = 0;
            if (exam.FlagPercent.Equals("grade"))
            {
                exam.EasyPercent = exam.OrdinaryGrade;
                exam.MediumPercent = exam.GoodGrade - exam.OrdinaryGrade;
                exam.HardPercent = 100 - exam.EasyPercent - exam.MediumPercent;
            }
            //if (exam.FlagPercent.Equals("grade"))
            //{
            //    double minError = 0;
            //    for (int i = 0; i <= 100; i++)
            //    {
            //        for (int j = 0; j <= (100 - i); j++)
            //        {
            //            int hardQuestionPercentTmp = 100 - i - j;
            //            double ordinaryStudentGradeTmp = ORDINARY_STUDENT_EASY_PERCENT * i + ORDINARY_STUDENT_MEDIUM_PERCENT * j + ORDINARY_STUDENT_HARD_PERCENT * hardQuestionPercentTmp;
            //            double goodStudentGradeTmp = GOOD_STUDENT_EASY_PERCENT * i + GOOD_STUDENT_MEDIUM_PERCENT * j + GOOD_STUDENT_HARD_PERCENT * hardQuestionPercentTmp;
            //            double excellentStudentGradeTmp = EXCELLENT_STUDENT_EASY_PERCENT * i + EXCELLENT_STUDENT_MEDIUM_PERCENT * j + EXCELLENT_STUDENT_HARD_PERCENT * hardQuestionPercentTmp;
            //            double minErrorTmp = Math.Abs(exam.OrdinaryGrade - ordinaryStudentGradeTmp) + Math.Abs(exam.GoodGrade - goodStudentGradeTmp) + Math.Abs(exam.ExcellentGrade - excellentStudentGradeTmp);
            //            if ((minErrorTmp < minError) || (i == 0 && j == 0))
            //            {
            //                exam.EasyPercent = i;
            //                exam.MediumPercent = j;
            //                exam.HardPercent = hardQuestionPercentTmp;
            //                minError = minErrorTmp;
            //            }
            //        }
            //    }
            //}
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
            LearingOutcomeInExamination firstTopic = topics.FirstOrDefault();

            if (firstTopic != null)
            {

                courseId = learningOutcomeService.GetCourseIdByLearningOutcomeId(firstTopic.Id);

            }
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
                if(exam.Semeter != 0)
                {
                    examination.SemesterId = exam.Semeter;
                }
                unitOfWork.Repository<Examination>().Insert(examination);
                unitOfWork.SaveChanges();
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
            List<QuestionViewModel> questionEasy = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, learingOutcomeInExam.EasyQuestion, EASY);
            List<QuestionViewModel> questionMedium = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, learingOutcomeInExam.MediumQuestion, MEDIUM);
            List<QuestionViewModel> questionHard = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, learingOutcomeInExam.HardQuestion, HARD);
            List<QuestionViewModel> result = questionEasy.Concat(questionMedium).Concat(questionHard).ToList();
            return result;
        }
        private List<QuestionViewModel> GeneratePartOfExamByLearningOutcomeAndLevel(int learningOutcomeId, int numberOfQuestion, string nameOfLevel)
        {
            List<QuestionViewModel> result = new List<QuestionViewModel>();
            int idOfLevel = levelService.GetIdByName(nameOfLevel);
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            for (int j = 0; j < 2; j++)
            {
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
                    unitOfWork.SaveChanges();
                }
                foreach (QuestionViewModel ques in result)
                {
                    Question questionEntity = questionsByLevelAndLearningOutcome.Where(q => q.Id == ques.Id).FirstOrDefault();
                    questionEntity.Frequency = questionEntity.Frequency + 1;
                    questionEntity.Priority = 0;
                    unitOfWork.Repository<Question>().Update(questionEntity);
                    unitOfWork.SaveChanges();
                }                         
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
                                        .Select(q => new QuestionInExamViewModel()
                                        {
                                            Id = q.Id,
                                            Frequency = q.Frequency.Value,
                                            Priority = q.Priority.Value,
                                            QuestionContent = q.QuestionContent,
                                            LevelId = q.LevelId.HasValue ? q.LevelId.Value : 0,
                                            LearningOutcome = q.LearningOutcomeId.HasValue ? q.LearningOutcome.Name : "",
                                            Image = q.Image,
                                            QuestionCode = q.QuestionCode,
                                            Options = q.Options.Select(o => new OptionViewModel
                                            {
                                                Id = o.Id,
                                                Image = o.Image,
                                                IsCorrect = o.IsCorrect.HasValue && o.IsCorrect.Value,
                                                OptionContent = o.OptionContent
                                            }).ToList()
                                        })
                                        .OrderBy(q => q.Priority)
                                        .ThenByDescending(q => q.Frequency)
                                        .ToList();


            return listQuestion;

        }
    }
}
