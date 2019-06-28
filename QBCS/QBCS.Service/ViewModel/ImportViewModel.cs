using QBCS.Service.Enum;
using System;

namespace QBCS.Service.ViewModel
{
    public class ImportViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalQuestion { get; set; }
        public StatusEnum Status { get; set; }
        public int TotalSuccess { get; set; }
        public string OwnerName { get; set; }
    }
}