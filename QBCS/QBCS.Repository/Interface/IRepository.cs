using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Interface
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(object id);
        void Insert(T entity);
        T InsertAndReturn(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetNoTracking();
    }
}
