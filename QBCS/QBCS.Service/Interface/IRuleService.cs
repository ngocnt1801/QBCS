using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IRuleService
    {
        List<RuleViewModel> GetAllRule();
        bool UpdateRule(List<RuleAjaxHandleViewModel> rule);
        RuleValueViewModel GetRuleById(int id);
    }
}
