﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion.Entity
{
    public class OptionModel
    {
        public string OptionContent { get; set; }
        public string Image { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public int Id { get; set; }
        public List<ImageModel> Images { get; set; }
        public  int OptionId { get; set; }
    }
}
