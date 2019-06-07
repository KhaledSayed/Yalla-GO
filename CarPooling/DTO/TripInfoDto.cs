using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class TripInfoDto
    {
        public Leg Info { get; set; }
        public OverviewPolyline EncodedRoute { get; set; }

        public TripRangePayment ExcpectedPayment { get; set; }
    }
}
