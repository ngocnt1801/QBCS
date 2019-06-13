using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion.Entity
{
    public class ImportModel
    {
        public int ImportId { get; set; }
        public int Status { get; set; }
        public int CourseId { get; set; }
        public int TotalSuccess { get; set; }
    }
}
