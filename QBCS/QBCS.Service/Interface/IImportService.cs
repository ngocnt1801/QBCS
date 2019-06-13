using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IImportService
    {
        ImportResultViewModel GetImportResult(int importId);
        void UpdateQuestionTemp(QuestionTempViewModel question);
        QuestionTempViewModel GetQuestionTemp(int questionTempId);
        Task ImportToBank(int importId);
        void Cancel(int importId);
        List<QuestionTempViewModel> CheckRule(List<QuestionTempViewModel> tempQuestions);
    }
}
