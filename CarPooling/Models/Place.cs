using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class Place
    {
        [Key]
        public int Id { get; set; }

        public IPoint Location { get; set; }

        public string Name { get; set; }

    }
}
