using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class RuleValueViewModel
    {
        public int Id { get; set; }
        public int KeyId { get; set; }
        public string Value { get; set; }
        public DateTime ActivateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ValueGroup { get; set; }
        public bool IsCaseSensitive { get; set; }
        public bool IsSet { get; set; }
    }
}
