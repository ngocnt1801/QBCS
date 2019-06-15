﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class LogViewModel
    {
        public int UserId { get; set; }
        public DateTime LogDate { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public string TargetName { get; set; }
        public int? TargetId { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
