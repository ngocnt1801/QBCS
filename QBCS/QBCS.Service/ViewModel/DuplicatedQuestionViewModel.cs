using QBCS.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class DuplicatedQuestionViewModel
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public bool IsBank { get; set; }
        public StatusEnum Status { get; set; }
    }
}
