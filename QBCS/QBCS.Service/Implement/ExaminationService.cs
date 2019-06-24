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
        private const double ORDINARY_STUDENT_EASY_PERCENT = 0.8;
        private const double ORDINARY_STUDENT_MEDIUM_PERCENT = 0.5;
        private const double ORDINARY_STUDENT_HARD_PERCENT = 0.1;

        private const double GOOD_STUDENT_EASY_PERCENT = 0.9;
        private const double GOOD_STUDENT_MEDIUM_PERCENT = 0.7;
        private const double GOOD_STUDENT_HARD_PERCENT = 0.3;

        private const double EXCELLENT_STUDENT_EASY_PERCENT = 0.9;
        private const double EXCELLENT_STUDENT_MEDIUM_PERCENT = 0.8;
        private const double EXCELLENT_STUDENT_HARD_PERCENT = 0.6;

        private const string EASY = "Easy";
        private const string MEDIUM = "Medium";
        private const string HARD = "Hard";
        private IUnitOfWork unitOfWork;
        private ILevelService levelService;
        private IQuestionService questionService;
        private ILearningOutcomeService learningOutcomeService;
        private ITopicService topicService;
        private int courseId;
        public ExaminationService()
        {
            unitOfWork = new UnitOfWork();
            levelService = new LevelService();
            questionService = new QuestionService();
            learningOutcomeService = new LearningOutcomeService();
            topicService = new TopicService();
        }
        public GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam)
        {
            if (exam.FlagPercent.Equals("grade"))
            {
                double minError = 0;
                for (int i = 0; i <= 100; i++)
                {
                    for (int j = 0; j <= (100 - i); j++)
                    {
                        int hardQuestionPercentTmp = 100 - i - j;
                        double ordinaryStudentGradeTmp = ORDINARY_STUDENT_EASY_PERCENT * i + ORDINARY_STUDENT_MEDIUM_PERCENT * j + ORDINARY_STUDENT_HARD_PERCENT * hardQuestionPercentTmp;
                        double goodStudentGradeTmp = GOOD_STUDENT_EASY_PERCENT * i + GOOD_STUDENT_MEDIUM_PERCENT * j + GOOD_STUDENT_HARD_PERCENT * hardQuestionPercentTmp;
                        double excellentStudentGradeTmp = EXCELLENT_STUDENT_EASY_PERCENT * i + EXCELLENT_STUDENT_MEDIUM_PERCENT * j + EXCELLENT_STUDENT_HARD_PERCENT * hardQuestionPercentTmp;
                        double minErrorTmp = Math.Abs(exam.OrdinaryGrade - ordinaryStudentGradeTmp) + Math.Abs(exam.GoodGrade - goodStudentGradeTmp) + Math.Abs(exam.ExcellentGrade - excellentStudentGradeTmp);
                        if ((minErrorTmp < minError) || (i == 0 && j == 0))
                        {
                            exam.EasyPercent = i;
                            exam.MediumPercent = j;
                            exam.HardPercent = hardQuestionPercentTmp;
                            minError = minErrorTmp;
                        }
                    }
                }
            }
            int questionEasy = (exam.TotalQuestion * exam.EasyPercent) / 100;
            int questionMedium = (exam.TotalQuestion * exam.MediumPercent) / 100;
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
                    totalEasyQuestionInTopic = questionService.GetCountOfListQuestionByLearningOutcomeAndId(id, idOfLevel, exam.CategoryId);
                    totalEasyQuestionInTopicCategory += totalEasyQuestionInTopic;

                    idOfLevel = levelService.GetIdByName(MEDIUM);
                    totalMediumQuestionInTopic = questionService.GetCountOfListQuestionByLearningOutcomeAndId(id, idOfLevel, exam.CategoryId);
                    totalMediumQuestionInTopicCategory += totalMediumQuestionInTopic;

                    idOfLevel = levelService.GetIdByName(HARD);
                    totalHardQuestionInTopic = questionService.GetCountOfListQuestionByLearningOutcomeAndId(id, idOfLevel, exam.CategoryId);
                    totalHardQuestionInTopicCategory += totalHardQuestionInTopic;
                }
                else
                {
                    int idOfLevel = levelService.GetIdByName(EASY);
                    totalEasyQuestionInTopic = questionService.GetCountOfListQuestionByTopicAndId(id, idOfLevel, exam.CategoryId);
                    totalEasyQuestionInTopicCategory += totalEasyQuestionInTopic;

                    idOfLevel = levelService.GetIdByName(MEDIUM);
                    totalMediumQuestionInTopic = questionService.GetCountOfListQuestionByTopicAndId(id, idOfLevel, exam.CategoryId);
                    totalMediumQuestionInTopicCategory += totalMediumQuestionInTopic;

                    idOfLevel = levelService.GetIdByName(HARD);
                    totalHardQuestionInTopic = questionService.GetCountOfListQuestionByTopicAndId(id, idOfLevel, exam.CategoryId);
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
            }
            else
            {
                exam.EasyQuestionGenerrate = questionEasy;
            }
            if (questionHard > totalHardQuestionInTopicCategory)
            {
                questionHard = totalHardQuestionInTopicCategory;
                exam.HardQuestionGenerrate = totalHardQuestionInTopicCategory;
            }
            else
            {
                exam.HardQuestionGenerrate = questionHard;
            }
            if (questionMedium > totalMediumQuestionInTopicCategory)
            {
                questionMedium = totalMediumQuestionInTopicCategory;
                exam.MediumQuestionGenerrate = totalMediumQuestionInTopicCategory;
            }
            else
            {
                exam.MediumQuestionGenerrate = questionMedium;
            }
            exam.TotalQuestionGenerrate = questionEasy + questionHard + questionMedium;
            LearingOutcomeInExamination firstTopic = topics.FirstOrDefault();

            if (firstTopic != null)
            {

                courseId = learningOutcomeService.GetCourseIdByLearningOutcomeId(firstTopic.Id);

            }
            Examination examination = new Examination()
            {
                CourseId = courseId,
                NumberOfHard = questionHard,
                NumberOfEasy = questionEasy,
                NumberOfMedium = questionMedium,
                GeneratedDate = DateTime.Now,
            };
            unitOfWork.Repository<Examination>().Insert(examination);
            unitOfWork.SaveChanges();
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
                        QuestionCode = ques.QuestionCode,
                        LevelId = ques.LevelId,
                        CategoryId = ques.CategoryId,
                        OptionInExams = ques.Options.Select(o => new OptionInExam()
                        {
                            IsCorrect = o.IsCorrect,
                            OptionContent = o.OptionContent
                        }).ToList()
                    };
                    unitOfWork.Repository<QuestionInExam>().Insert(question);
                    unitOfWork.SaveChanges();
                }
            }
            exam.ExamId = examination.Id;
            exam.calculateGrade();
            return exam;
        }

        private List<QuestionViewModel> GeneratePartOfExamByLearningOutcome(LearingOutcomeInExamination learingOutcomeInExam, int categoryId)
        {
            List<QuestionViewModel> questionEasy = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, categoryId, learingOutcomeInExam.EasyQuestion, EASY);
            List<QuestionViewModel> questionMedium = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, categoryId, learingOutcomeInExam.MediumQuestion, MEDIUM);
            List<QuestionViewModel> questionHard = GeneratePartOfExamByLearningOutcomeAndLevel(learingOutcomeInExam.Id, categoryId, learingOutcomeInExam.HardQuestion, HARD);
            List<QuestionViewModel> result = questionEasy.Concat(questionMedium).Concat(questionHard).ToList();
            return result;
        }
        private List<QuestionViewModel> GeneratePartOfExamByLearningOutcomeAndLevel(int learningOutcomeId, int categoryId, int numberOfQuestion, string nameOfLevel)
        {
            List<QuestionViewModel> result = new List<QuestionViewModel>();
            int idOfLevel = levelService.GetIdByName(nameOfLevel);
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            for (int j = 0; j < 2; j++)
            {
                int minFrequency = questionService.GetMinFreQuencyByLearningOutcome(learningOutcomeId, idOfLevel, categoryId);
                List<Question> questionsByLevelAndLearningOutcome = questions.Where(q => q.LevelId == idOfLevel && q.LearningOutcomeId == learningOutcomeId && q.CategoryId == categoryId).ToList();
                List<QuestionViewModel> questionViewModelRemoveRecent = questionsByLevelAndLearningOutcome.Where(q => q.Frequency == minFrequency && q.Priority != 0).Select(c => new QuestionViewModel
                {
                    Frequency = c.Frequency.HasValue ? (int)c.Frequency : 0,
                    Id = c.Id,
                    LevelId = c.LevelId.HasValue ? (int)c.LevelId : 0,
                    LearningOutcomeId = c.LearningOutcomeId.HasValue ? (int)c.LearningOutcomeId : 0,
                    Priority = c.Priority.HasValue ? (int)c.Priority : 0,
                    QuestionContent = c.QuestionContent,
                    QuestionCode = c.QuestionCode,
                    CategoryId = c.CategoryId.HasValue ? (int)c.CategoryId : 0,
                    Options = c.Options.Select(d => new OptionViewModel
                    {
                        Id = d.Id,
                        OptionContent = d.OptionContent,
                        IsCorrect = (bool)d.IsCorrect
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
                            int randomIndex = r.Next(0, listQuestionRemoveRecent[i].Count);
                            var question = listQuestionRemoveRecent[i][randomIndex];
                            listQuestionRemoveRecent[i].RemoveAt(randomIndex);
                            QuestionViewModel questionViewModel = questionViewModelRemoveRecent.Where(q => q.Id == question.Id).FirstOrDefault();
                            result.Add(questionViewModel);
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
                }
                foreach (QuestionViewModel ques in result)
                {
                    Question questionEntity = questionsByLevelAndLearningOutcome.Where(q => q.Id == ques.Id).FirstOrDefault();
                    questionEntity.Frequency = questionEntity.Frequency + 1;
                    questionEntity.Priority = 0;
                }
                foreach (Question questionEntity in questionsByLevelAndLearningOutcome)
                {
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
    }
}
