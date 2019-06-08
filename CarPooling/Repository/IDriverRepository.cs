using CarPooling.Models;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public interface IDriverRepository : IBaseRepository<Driver>
    {
       Task<List<Driver>>  FindAll();
       Task<Driver> FindOneById(int Id);

        Task<Driver> NearestOnlineAndFreeDriver(IPoint nearestPoint,double distance);

        Task<bool> IsDriverOfTrip(int tripId, int driverId);

        Task<Driver> FindDriverWithConnectionId(string connectionId);
    }
}
