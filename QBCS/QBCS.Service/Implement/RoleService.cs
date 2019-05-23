using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class RoleService
    {
        private IUnitOfWork unitOfWork;

        public RoleService()
        {
            unitOfWork = new UnitOfWork();
        }

        public Role GetRoleByName(string)
    }
}
