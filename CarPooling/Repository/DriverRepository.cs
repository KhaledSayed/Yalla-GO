using CarPooling.Data;
using CarPooling.Models;
using CarPooling.Models.enums;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public class DriverRepositoy : BaseRepository<Driver>, IDriverRepository
    {
        private readonly DataContext context;

        public DriverRepositoy(DataContext Context) : base(Context)
        {
            context = Context;
        }

        public async Task<List<Driver>> FindAll()
        {
            return await context.Drivers.ToListAsync();
        }

        public async Task<Driver> FindOneById(int Id)
        {
       
                return await context.Drivers.FirstOrDefaultAsync(c => c.Id == Id);
         
           

        }

        public async Task<Driver> NearestOnlineAndFreeDriver(double distance, IPoint nearestPoint)
        {
            return await context.Drivers
                .Include(d => d.Trips)
                .FirstOrDefaultAsync(d => d.Status == Status.ONLINE && d.Location.Distance(nearestPoint) < distance &&
                 d.Trips.Where(t => t.Status != TripStatus.ENDED).Count() > 0
                );
        }
    }
}
