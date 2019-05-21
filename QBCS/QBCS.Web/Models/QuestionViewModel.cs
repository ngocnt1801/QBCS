using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBCS.Web.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public List<Option> Options { get; set; }
    }
}