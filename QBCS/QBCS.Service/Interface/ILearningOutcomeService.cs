using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ILearningOutcomeService
    {
        List<LearningOutcomeViewModel> GetLearningOutcomeByCourseId(int CourseId);
        List<LearningOutcomeViewModel> GetAllLearningOutcome();
        int UpdateDisable(int id);
        int UpdateLearningOutcome(LearningOutcomeViewModel model);
        LearningOutcomeViewModel GetLearningOutcomeById(int id);
        int AddLearningOutcome(LearningOutcomeViewModel model);
        //List<LearningOutcomeViewModel> GetLearningOutcomeByCourseId(int CourseId);
        int GetCourseIdByLearningOutcomeId(int learningOutcomeId);
        LearningOutcomeViewModel GetLearingOutcomeById(int learningOutcomeId);

        CourseViewModel GetCourseByLearningOutcome(int id);
    }
}
