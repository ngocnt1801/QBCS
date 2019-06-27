using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IExaminationService
    {
        GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam);
        List<ExaminationViewModel> GetAllExam();
    }
}
