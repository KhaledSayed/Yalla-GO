using CarPooling.Models;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public interface ITripRepository : IBaseRepository<Trip>
    {
        Task<List<Trip>>  FindAll();
       Task<Trip> FindOneById(int Id);

        Task<Trip> FindTripNearestByLocation(IPoint originPoint,IPoint destintPoint);
    }
}
