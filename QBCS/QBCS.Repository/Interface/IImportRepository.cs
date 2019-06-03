using QBCS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Interface
{
    public interface IImportRepository : IRepository<Import>
    {
        int GetNotifyImportResult(int userId);
    }
}
