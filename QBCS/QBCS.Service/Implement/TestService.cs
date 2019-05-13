using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    class TestService
    {
        private IUnitOfWork u;
        public TestService()
        {
            u = new UnitOfWork();
        }

    }
}
