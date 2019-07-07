using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ISemesterService
    {
        List<SemesterViewModel> GetAllSemester();
        SemesterViewModel GetById(int semesterId);
    }
}
