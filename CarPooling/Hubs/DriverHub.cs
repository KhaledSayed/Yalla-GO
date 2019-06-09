
using CarPooling.DTO;
using GeoAPI.Geometries;
using Microsoft.AspNetCore.SignalR;
using NetAd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Hubs
{
    public class DriverHub: Hub
    {
        private readonly IDriverRepository driverRepository;

        public DriverHub(IDriverRepository driverRepository)
        {
            this.driverRepository = driverRepository;
        }

        public async Task Online()
        {

            await Clients.All.SendAsync("Driver", "You recieved a trip");
        }

        public async Task Offline(int driverId, double lat, double lng)
        {
            var driver = await this.driverRepository.FindOneById(driverId);

            driver.Status = Models.enums.Status.ONLINE;
        }


    }
}
