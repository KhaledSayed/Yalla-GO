using CarPooling.Models.owned;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class ExpectedPoint
    {

        [Key]
        int Id; 

        public Item Distance { get; set; }

        public IPoint StartLocation { get; set; }

        public IPoint EndLocation { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
