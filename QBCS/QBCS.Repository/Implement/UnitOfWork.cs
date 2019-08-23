using QBCS.Entity;
using QBCS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext context;

        public UnitOfWork()
        {
            context = new QBCSContext();
        }

        public DbContext GetContext()
        {
            return this.context;
        }

        public IImportRepository ImportRepository()
        {
            return new ImportRepository(context);
        }

        public IExaminationRepository ExaminationRepository()
        {
            return new ExaminationRepository(context);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(context);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }
}
