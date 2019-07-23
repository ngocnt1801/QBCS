using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ISyllabusService
    {
        List<SyllabusPartialViewModel> GetSyllabusPartials(int courseId);
        List<LearningOutcomeViewModel> GetLearningOutcomes(int? syllabusPartialId);
        void DeleteSyllabusPartial(int syllabusId);
        void AddSyllabusPartial(SyllabusPartialViewModel model);
        void ChangeSyllabusPartial(int locId, int? syllabusParitalId);
        void UpdateSyllabusPartial(SyllabusPartialViewModel model);
    }
}
