﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public int Frequency { get; set; }
        public int Priority { get; set; }
        public int CourseId { get; set; }
        public List<OptionViewModel> Options { get; set; }
    }
}
