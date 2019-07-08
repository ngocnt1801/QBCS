using QBCS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Implement
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected DbContext context;
        protected DbSet<T> dbSet;

        public Repository(DbContext _context)
        {
            context = _context;
            dbSet = _context.Set<T>();
        }

        public void Delete(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public T GetById(object id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public T InsertAndReturn(T entity)
        {
            return dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public IQueryable<T> GetNoTracking()
        {
            return dbSet.AsNoTracking<T>();
        }
    }
}
