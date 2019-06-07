using CarPooling.Data;
using CarPooling.Models;
using CarPooling.Models.enums;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public class TripRepository : BaseRepository<Trip>, ITripRepository
    {
        private readonly DataContext context;

        private double AwayDistanceInM = 500;

        public TripRepository(DataContext Context) : base(Context)
        {
            context = Context;
        }

        public async Task<List<Trip>> FindAll()
        {
            return await context.Trips.ToListAsync();
        }

        public async Task<Trip> FindOneById(int Id)
        {

            return await context.Trips.FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task<Trip> FindTripNearestByLocation(IPoint originPoint,IPoint destPoint)
        {

            return await context.Trips
                .Include(t => t.ExpectedRoad)
                .Include(t => t.Clients)
                .Include(t => t.Points)
                .FirstOrDefaultAsync(t =>
                        t.Status != TripStatus.ENDED && t.Clients.Where(c => c.Status == ClientTripStatus.JOINED).Count() < 3 &&
                        t.ExpectedRoad.Where(p => p.Location.Distance(originPoint) < AwayDistanceInM).Count() >= 1 &&
                        t.FinalLocation.Location.Distance(destPoint) < AwayDistanceInM);
        }
    }
}
