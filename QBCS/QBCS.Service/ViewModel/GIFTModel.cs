using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    class GIFTModel
    {
        public string QuestionCode { get; set; }
        public string QuestionName { get; set; }
        public  List<OptionViewModel> Options { get; set; }
    }
}
