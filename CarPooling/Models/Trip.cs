using CarPooling.Models.enums;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        public ICollection<ClientTrip> Clients { get; set; }

        public Place StartLocation { get; set; }

        public Place FinalLocation { get; set; }

        public TripStatus Status { get; set; }

        public ICollection<TripPointAtTime> Points { get; set; }

        public ICollection<ExpectedPoint> ExpectedRoad { get; set; }

        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }

       

        public int? DriverId { get; set; }


        public Trip()
        {
            Clients = new Collection<ClientTrip>();
            Points = new Collection<TripPointAtTime>();
            ExpectedRoad = new Collection<ExpectedPoint>();
        }
    }
}
