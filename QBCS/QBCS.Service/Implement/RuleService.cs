using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;

namespace QBCS.Service.Implement
{
    public class RuleService: IRuleService
    {
        private IUnitOfWork unitOfWork;

        public RuleService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<RuleViewModel> getAllRule()
        {
            List<RuleViewModel> result = new List<RuleViewModel>();
            //List<RuleKey> rules = unitOfWork.Repository<>
            return result;
        }
        public bool UpdateRule(RuleViewModel rule)
        {
            return false;
        }
    }
}
