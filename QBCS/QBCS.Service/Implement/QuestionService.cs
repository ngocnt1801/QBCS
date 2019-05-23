using QBCS.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Service.ViewModel;

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
            List<Question> Questions = unitOfWork.Repository<Question>().GetAll().ToList();
            List<Question> QuestionsByCourse = Questions.Where(q => q.CourseId == CourseId).ToList();

            List<QuestionViewModel> QuestionViewModel = new List<QuestionViewModel>();

            foreach(var ques in QuestionsByCourse)
            {
                List<OptionViewModel> ovms = new List<OptionViewModel>();
                foreach (var option in ques.Options)
                {

                    OptionViewModel ovm = new OptionViewModel()
                    {
                        Id = option.Id,
                        OptionContent = option.OptionContent,
                        IsCorrect = (bool)option.IsCorrect
                    };
                    ovms.Add(ovm);
                }


                QuestionViewModel qvm = ParseEntityToModel(ques, ovms);
                QuestionViewModel.Add(qvm);
            }
            return QuestionViewModel;
        }

        public QuestionViewModel GetQuestionById (int id )
        {
            Question QuestionById = unitOfWork.Repository<Question>().GetById(id);

            List<OptionViewModel> ovms = new List<OptionViewModel>();
            foreach (var option in QuestionById.Options)
            {

                OptionViewModel ovm = new OptionViewModel()
                {
                    Id = option.Id,
                    OptionContent = option.OptionContent,
                    IsCorrect = (bool)option.IsCorrect
                };
                ovms.Add(ovm);
            }


            QuestionViewModel qvm = ParseEntityToModel(QuestionById, ovms);
            return qvm;
        }

        public bool UpdateQuestion(QuestionViewModel question)
        {
            Question ques = unitOfWork.Repository<Question>().GetById(question.Id);
            ques.QuestionContent = question.QuestionContent;
            ques.LevelId = question.LevelId;
            ques.LearningOutcomeId = question.LearningOutcomeId;
            ques.TopicId = question.TopicId;

            unitOfWork.Repository<Question>().Update(ques);
            unitOfWork.SaveChanges();
            return true;
        }

        private QuestionViewModel ParseEntityToModel ( Question question, List<OptionViewModel> options)
        {
            QuestionViewModel qvm = new ViewModel.QuestionViewModel()
            {
                Id = question.Id,
                QuestionContent = question.QuestionContent,
                Options = options
            };
            if (question.CourseId != null)
            {
                qvm.CourseId = (int)question.CourseId;
            }
            if (question.TopicId != null)
            {
                qvm.TopicId = (int)question.TopicId;
            }
            if (question.LevelId != null)
            {
                qvm.LevelId = (int)question.LevelId;
            }
            if (question.LearningOutcomeId != null)
            {
                qvm.LearningOutcomeId = (int)question.LearningOutcomeId;
            }

            return qvm;
        }
    }
}
