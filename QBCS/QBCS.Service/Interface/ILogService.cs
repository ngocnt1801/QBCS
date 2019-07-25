using QBCS.Entity;
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
        GetActivityViewModel GetAllActivities(string search, int start, int length);
        GetActivityViewModel GetAllActivitiesByTargetId(int targetId, string search, int start, int length);
        List<LogViewModel> GetListQuestionImportByTargetId(int targetId);
        LogViewModel GetQuestionImportByTargetId(int targetId);
        GetActivityViewModel GetAllActivitiesByUserId(int id, string search, int start, int length);
        IEnumerable<LogViewModel> GetActivitiesById(int id);
        QuestionViewModel ParseEntityToModel(Question question);
        bool UpdateLogStatus(int importId);
        void LogManually( string action, string targetName, int? targetId = null, int? userId = null, string controller = "", string method = "", string fullname = "", string usercode = "");
        void LogFullManually(string action, string targetName, int? targetId = null, int? userId = null, string controller = "", string method = "",
            string fullname = "", string usercode = "", string newValue = "", string oldValue = "");
    }
}
