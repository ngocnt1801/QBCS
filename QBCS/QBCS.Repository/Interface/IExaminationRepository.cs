using QBCS.Entity;
using QBCS.Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Interface
{
    public interface IExaminationRepository : IRepository<Examination>
    {
        ExaminationStatisticViewModel GetStatistic(int courseId);
    }
}
