using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion.Entity
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public int QuestionId { get; set; }
        public int QuestionTempId { get; set; }
        public int OptionTempId { get; set; }
        public int OptionId { get; set; }
    }
}
