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
    }
}
