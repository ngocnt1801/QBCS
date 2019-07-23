using System.Collections.Generic;

namespace QBCS.Service.ViewModel
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DepartmentId { get; set; }
        public int DefaultNumberOfQuestion { get; set; }
        public List<TopicViewModel> Topic { get; set; }
        public List<LearningOutcomeViewModel> LearningOutcome { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<SyllabusPartialViewModel> Syllabus { get; set; }
        public bool IsDisable { get; set; }
        public int Total { get; set; }
        public List<SemesterViewModel> Semester { get; set; }

    }
}
