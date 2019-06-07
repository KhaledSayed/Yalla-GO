using CarPooling.Models.enums;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class ClientTrip
    {

        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public Place FromLocation { get; set; }
        public Place ToLocation { get; set; }

        public string Duration { get; set; }

        public string Distance { get; set; }

        public string SuggestedPrice { get; set; }

        public ClientTripStatus Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime LeavedAt { get; set; }

        public ICollection<ClientTripPointAtTime> Points { get; set; }


        public ClientTrip()
        {
            Points = new Collection<ClientTripPointAtTime>();
        }

    }
}
