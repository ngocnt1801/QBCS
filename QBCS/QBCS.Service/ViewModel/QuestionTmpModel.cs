using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionTmpModel
    {
        public string Code { get; set; }
        public string QuestionContent { get; set; }
        public string OptionsContent { get; set; }
        public int Status { get; set; }
        public int DuplicatedId { get; set; }
    }
}
