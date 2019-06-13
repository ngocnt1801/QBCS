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

            int questionEasy = (exam.TotalQuestion * exam.EasyPercent) / 100;
            int questionMedium = (exam.TotalQuestion * exam.MediumPercent) / 100;
            int questionHard = exam.TotalQuestion - questionEasy - questionMedium;
            exam.EasyQuestion = questionEasy;
            exam.MediumQuestion = questionMedium;
            exam.HardQuestion = questionHard;
            List<TopicInExamination> topics = new List<TopicInExamination>();
            foreach (string topic in exam.Topic)
            {
                int id = int.Parse(topic.Substring(3));
                bool isLearingOutcome = false;
                if (topic.Contains("LO_"))
                {
                    isLearingOutcome = true;
                }
                TopicInExamination topicInExam = new TopicInExamination()
                {
                    Id = id,
                    IsLearingOutcome = isLearingOutcome
                };
                topics.Add(topicInExam);
            }
            TopicInExamination firstTopic = topics.FirstOrDefault();

            if (firstTopic != null)
            {
                if (firstTopic.IsLearingOutcome)
                {
                    courseId = learningOutcomeService.GetCourseIdByLearningOutcomeId(firstTopic.Id);
                }
                else
                {
                    courseId = topicService.GetCourseIdByTopicId(firstTopic.Id);
                }
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
                        topics[i].EasyQuestion = topics[i].EasyQuestion + 1;
                        questionEasy--;
                    }
                    else if (questionMedium != 0)
                    {
                        topics[i].MediumQuestion = topics[i].MediumQuestion + 1;
                        questionMedium--;
                    }
                    else if (questionHard != 0)
                    {
                        topics[i].HardQuestion = topics[i].HardQuestion + 1;
                        questionHard--;
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
                if (topic.IsLearingOutcome)
                {
                    questionInPartOfExam = GeneratePartOfExamByLearningOutcome(topic, exam.CategoryId);
                    partOfExam = new PartOfExamination()
                    {
                        LearningOutcomeId = topic.Id,
                        NumberOfQuestion = topic.EasyQuestion + topic.HardQuestion + topic.MediumQuestion,
                        ExaminationId = examination.Id
                    };
                }
                else
                {
                    questionInPartOfExam = GeneratePartOfExamByTopic(topic, exam.CategoryId);
                    partOfExam = new PartOfExamination()
                    {
                        TopicId = topic.Id,
                        NumberOfQuestion = topic.EasyQuestion + topic.HardQuestion + topic.MediumQuestion,
                        ExaminationId = examination.Id
                    };
                }
                unitOfWork.Repository<PartOfExamination>().Insert(partOfExam);
                unitOfWork.SaveChanges();
                foreach(var ques in questionInPartOfExam)
                {
                    QuestionInExam question = new QuestionInExam()
                    {
                        QuestionContent = ques.QuestionContent,
                        PartId = partOfExam.Id,
                        QuestionReference = ques.Id,
                        Priority = ques.Priority,
                        Frequency = ques.Frequency,
                        LevelId = ques.LevelId,
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
            return exam;
        }
        private List<QuestionViewModel> GeneratePartOfExamByTopic(TopicInExamination topicInExam, int categoryId)
        {
            List<QuestionViewModel> questionEasy = GeneratePartOfExamByTopicAndLevel(topicInExam.Id, categoryId, topicInExam.EasyQuestion, EASY);
            List<QuestionViewModel> questionMedium = GeneratePartOfExamByTopicAndLevel(topicInExam.Id, categoryId, topicInExam.MediumQuestion, MEDIUM);
            List<QuestionViewModel> questionHard = GeneratePartOfExamByTopicAndLevel(topicInExam.Id, categoryId, topicInExam.HardQuestion, HARD);
            List<QuestionViewModel> result = questionEasy.Concat(questionMedium).Concat(questionHard).ToList();
            return result;
        }

        private List<QuestionViewModel> GeneratePartOfExamByTopicAndLevel(int topicId, int categoryId, int numberOfQuestion, string nameOfLevel)
        {
            List<QuestionViewModel> result = new List<QuestionViewModel>();
            int idOfLevel = levelService.GetIdByName(nameOfLevel);
            IQueryable<Question> questions = unitOfWork.Repository<Question>().GetAll();
            for (int j = 0; j < 2; j++)
            {
                int minFrequency = questionService.GetMinFreQuencyByTopicAndLevel(topicId, idOfLevel);
                List<Question> questionsByLevelAndTopic = questions.Where(q => q.LevelId == idOfLevel && q.TopicId == topicId && q.CategoryId == categoryId).ToList();
                List<QuestionViewModel> questionViewModelRemoveRecent = questionsByLevelAndTopic.Where(q => q.Frequency == minFrequency && q.Priority != 0).Select(c => new QuestionViewModel
                {
                    Frequency = (int)c.Frequency,
                    Id = c.Id,
                    LevelId = (int)c.LevelId,
                    TopicId = (int)c.TopicId,
                    Priority = (int)c.Priority,
                    QuestionContent = c.QuestionContent,
                    Options = c.Options.Select(d => new OptionViewModel
                    {
                        Id = d.Id,
                        OptionContent = d.OptionContent,
                        IsCorrect = (bool)d.IsCorrect
                    }).ToList()
                }).ToList();
                var listQuestionRemoveRecent = questionsByLevelAndTopic
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
                foreach (Question questionEntity in questionsByLevelAndTopic)
                {
                    questionEntity.Priority = questionEntity.Priority + 1;
                }
                foreach (QuestionViewModel ques in result)
                {
                    Question questionEntity = questionsByLevelAndTopic.Where(q => q.Id == ques.Id).FirstOrDefault();
                    questionEntity.Frequency = questionEntity.Frequency + 1;
                    questionEntity.Priority = 0;
                }
                foreach (Question questionEntity in questionsByLevelAndTopic)
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

        private List<QuestionViewModel> GeneratePartOfExamByLearningOutcome(TopicInExamination learingOutcomeInExam, int categoryId)
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
                int minFrequency = questionService.GetMinFreQuencyByLearningOutcome(learningOutcomeId, idOfLevel);
                List<Question> questionsByLevelAndLearningOutcome = questions.Where(q => q.LevelId == idOfLevel && q.LearningOutcomeId == learningOutcomeId && q.CategoryId == categoryId).ToList();
                List<QuestionViewModel> questionViewModelRemoveRecent = questionsByLevelAndLearningOutcome.Where(q => q.Frequency == minFrequency && q.Priority != 0).Select(c => new QuestionViewModel
                {
                    Frequency = (int)c.Frequency,
                    Id = c.Id,
                    LevelId = (int)c.LevelId,
                    LearningOutcomeId = (int)c.LearningOutcomeId,
                    Priority = (int)c.Priority,
                    QuestionContent = c.QuestionContent,
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
