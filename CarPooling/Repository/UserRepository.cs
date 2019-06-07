using CarPooling.Data;
using CarPooling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext Context) : base(Context)
        {
            context = Context;
        }

        public async Task<List<User>> FindAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> FindOneById(int Id)
        {
                return await context.Users.FirstOrDefaultAsync(c => c.Id == Id);
        }
    }
}
