using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class ScriptService : IScriptService
    {
        private IUnitOfWork unitOfWork;

        public ScriptService()
        {
            unitOfWork = new UnitOfWork();
        }

        public void RunScirpt(string raw)
        {
            unitOfWork.GetContext().Database.ExecuteSqlCommand(raw);
        }
    }
}
