using DuplicateQuestion.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionTempViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public QuestionViewModel DuplicatedQuestion { get; set; }
        public string QuesitonContent { get; set; }
        public StatusEnum Status { get; set; }
        public int ImportId { get; set; }
        public List<OptionViewModel> Options { get; set; }
    }
}
