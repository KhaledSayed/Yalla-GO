using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class TripCheckQuery
    {

        public double OriginLat { get; set; }
        public double OriginLng { get; set; }

        public double DestinationLat { get; set; }
        public double DestinationLng { get; set; }
    }
}
