using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            bool result = true;
            //***
            //code check duplicate here
            //***

            return result;
        }
    }
}
