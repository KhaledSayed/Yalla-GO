using CarPooling.Data;
using NetAd.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        public DataContext Context { get; set; }

        public BaseRepository(DataContext Context)
        {
            this.Context = Context;
        }

        public void Add(T entity)
        {
            Context.Add(entity);
        }

        public void Delete(T entity)
        {
            Context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await Context.SaveChangesAsync() >  0;    
        }

      
        public void Update(T entity)
        {
            Context.Update(entity);
        }
    }
}
