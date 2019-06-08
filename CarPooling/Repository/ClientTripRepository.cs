using CarPooling.Data;
using CarPooling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public class ClientTripRepository : BaseRepository<ClientTrip>, IClientTripRepository
    {
        private readonly DataContext context;

        public ClientTripRepository(DataContext Context) : base(Context)
        {
            context = Context;
        }

        public async Task<List<ClientTrip>> FindAll()
        {
            return await context.ClientTrips.ToListAsync();
        }

        public async Task<ClientTrip> FindOneById(int Id)
        {
                return await context.ClientTrips.FirstOrDefaultAsync(c => c.Id == Id);
        }
    }
}
