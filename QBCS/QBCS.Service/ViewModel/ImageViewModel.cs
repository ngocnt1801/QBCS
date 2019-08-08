using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class ImageViewModel
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public int? QuestionId { get; set; }

        public int? QuestionTempId { get; set; }

        public int? QuestionInExamId { get; set; }
        public int? OptionId { get; set; }

        public int? OptionTempId { get; set; }

        public int? OptionInExamId { get; set; }
    }
}
