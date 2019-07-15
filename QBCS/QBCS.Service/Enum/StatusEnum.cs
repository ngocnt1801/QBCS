using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Enum
{
    public enum StatusEnum
    {
        NotCheck = 1,
        Editable = 2,
        Deleted = 3,
        Success = 4,
        Checked = 5,
        Editing = 6,
        Done = 7,
        Canceled = 8,
        DeleteOrSkip = 9,
        Invalid = 10
    }
}
