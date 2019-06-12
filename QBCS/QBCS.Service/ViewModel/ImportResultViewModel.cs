using System.Collections.Generic;

namespace QBCS.Service.ViewModel
{
    public class ImportResultViewModel
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int NumberOfSuccess { get; set; }
        public int NumberOfFail { get; set; }
        public List<QuestionTempViewModel> Questions { get; set; }
    }
}
