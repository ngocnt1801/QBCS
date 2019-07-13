
using QBCS.Service.Enum;
using System.Collections.Generic;

namespace QBCS.Service.ViewModel
{
    public class QuestionTempViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public QuestionViewModel DuplicatedQuestion { get; set; }
        public bool IsInImportFile { get; set; }
        public string QuestionContent { get; set; }
        public StatusEnum Status { get; set; }
        public int ImportId { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public string Category { get; set; }
        public List<DuplicatedQuestionViewModel> DuplicatedList { get; set; }
        public bool IsHide { get; set; }
    }
}
