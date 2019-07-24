using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GetLogActionViewModel
    {
        public List<LogViewModel> Logs { get; set; }
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
    }
}
