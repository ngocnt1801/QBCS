using QBCS.Entity;
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
        Task CheckDuplicateQuestion(int questionId, int logId);
        void Cancel(int importId);
        void UpdateQuestionTempStatus(int questionTempId, int status);
        List<ImportViewModel> GetListImport(int? userId);
        List<QuestionTemp> CheckRule(List<QuestionTemp> tempQuestions);
        QuestionTempViewModel GetDuplicatedDetail(int questionTempId);
        void RecoveryQuestionTemp(int questionTempId);
    }
}
