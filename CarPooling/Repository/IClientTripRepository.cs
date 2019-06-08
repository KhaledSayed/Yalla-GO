using CarPooling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetAd.Repository
{
    public interface IClientTripRepository : IBaseRepository<ClientTrip>
    {
       Task<List<ClientTrip>>  FindAll();
       Task<ClientTrip> FindOneById(int Id);
    }
}
