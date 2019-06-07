using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class ClientTripPointAtTime
    {
        [Key]
        public int Id { get; set; }

        public IPoint Location { get; set; }

        public DateTime Time { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public Trip Trip { get; set; }

        public int TripId { get; set; }

    }
}
