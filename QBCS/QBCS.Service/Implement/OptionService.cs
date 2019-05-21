using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    
    public class OptionService : IOptionService
    {
        private IUnitOfWork u;
        public OptionService()
        {
            u = new UnitOfWork();
        }

        public List<Option> GetOptionsByQuestion(int QuestionId)
        {
            List<Option> Options = u.Repository<Option>().GetAll().ToList();
            List<Option> OptionsByQuestion = (from o in Options
                                             where o.QuestionId == QuestionId
                                             select o).ToList();
            return OptionsByQuestion;
        }
    }
}
