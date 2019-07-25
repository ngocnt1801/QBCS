﻿using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ILogActionService
    {
        void LogAction(LogViewModel model);
        GetLogActionViewModel GetLogAction(string search, int start, int length);
    }
}
