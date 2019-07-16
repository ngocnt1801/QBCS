using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GetResultViewModel
    {
        public int ImportId { get; set; }
        public int totalNumber { get; set; }
        public int editableNumber { get; set; }
        public int successNumber { get; set; }
        public int invalidNumber { get; set; }
        public int deleteNumber { get; set; }
        public int NotInsertNumber { get; set; }
        public int RecheckNumber { get; set; }
    }
}
