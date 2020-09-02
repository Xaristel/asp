using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetAll();
        T GetByID(int id);
        void Add(T item);
        void Delete(T item);
        void Update(T item);
        void Save();
    }
}
