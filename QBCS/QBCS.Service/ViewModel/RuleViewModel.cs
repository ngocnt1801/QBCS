using QBCS.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class RuleViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<RuleValueViewModel> Value { get; set; }
        public int GroupType { get; set; }
        public RuleEnum GroupTypeEnum { get; set; }
}
}
