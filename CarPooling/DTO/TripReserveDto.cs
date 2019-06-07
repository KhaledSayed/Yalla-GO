using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class TripReserveDto
    {
        public Location OriginLocation { get; set; }

        public string OriginAddress { get; set; }
        public Location DistanceLocation { get; set; }
        public string DistantAddress { get; set; }

    }
}
