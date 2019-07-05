﻿using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ILogService
    {
        void Log(LogViewModel model);
        IEnumerable<LogViewModel> GetAllActivities();
        List<LogViewModel> GetAllActivitiesByTargetId(int targetId);
        List<LogViewModel> GetListQuestionImportByTargetId(int targetId);
        LogViewModel GetQuestionImportByTargetId(int targetId);
        List<LogViewModel> GetAllActivitiesByUserId(int id);
        IEnumerable<LogViewModel> GetActivitiesById(int id);
        QuestionViewModel ParseEntityToModel(Question question);
        void LogManually(int targetId,  string action, string targetName, int? userId = null, string controller = "", string method = "", string fullname = "", string usercode = "");
    }
}
