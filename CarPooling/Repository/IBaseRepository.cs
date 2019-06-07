using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public interface IBaseRepository<T>
    {
        void Add(T entity);
        void Delete(T entity);

        void Update(T entity);
        Task<bool> SaveAll();


    }
}
