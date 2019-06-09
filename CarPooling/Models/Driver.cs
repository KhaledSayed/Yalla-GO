using CarPooling.Models.enums;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class Driver : User
    {

        public ICollection<Trip> Trips { get; set; }

        public IPoint Location { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime LastOnlineAt { get; set; }

     //   [ForeignKey("CarId")]
     //   public Car CurrentCar { get; set; }

       
        //public int? CarId { get; set; }
        public ICollection<Connection> Connections { get; set; }

        public Status Status { get; set; }
        public Driver()
        {
            Trips = new Collection<Trip>();
            Connections = new Collection<Connection>();
        }
    }
}
