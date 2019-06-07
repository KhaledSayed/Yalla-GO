using CarPooling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
       Task<List<User>>  FindAll();
       Task<User> FindOneById(int Id);
    }
}
