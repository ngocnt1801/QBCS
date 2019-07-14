using QBCS.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class LogViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserRole { get; set; }
        public DateTime LogDate { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public string TargetName { get; set; }
        public int? TargetId { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Fullname { get; set; }
        public string UserCode { get; set; }
        public string OwnerName { get; set; }
        public string CourseCode { get; set; }
        public int Status { get; set; }
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        public int LevelId { get; set; }
        public int LearningOutcomeId { get; set; }
        public string Ip { get; set; }
        public string Route { get; set; }
        public string TimeAgo { get; set; }
        
        public QuestionViewModel QuestionNew { get; set; }
        public QuestionViewModel QuestionOld { get; set; }
        public List<QuestionViewModel> listQuestion { get; set; }

    }
}
