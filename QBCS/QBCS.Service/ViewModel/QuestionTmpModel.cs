﻿using QBCS.Entity;
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
        public string QuestionContentError { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public int Status { get; set; }
        public int DuplicatedId { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public string LearningOutcome { get; set; }
        public string Level { get; set; }
        //may delete
        public string Image { get; set; }
        public List<ImageViewModel> Images { get; set; }
        
        public string Error { get; set; }
        public string OtherError { get; set; }
        public string Message { get; set; }
    }
}
