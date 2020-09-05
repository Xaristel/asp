using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetByID(int id)
        {
            return context.Set<T>().Find(id);
        }

        public bool Add(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddRange(List<T> list)
        {
            context.Set<T>().AddRange(list);
        }

        public void Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
