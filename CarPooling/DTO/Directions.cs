using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class Directions
    {
        [JsonProperty("routes")]
        public List<Route>  Routes { get; set; }
    }

    public class Route
    {

        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }

        [JsonProperty("overview_polyline")]
        public OverviewPolyline OverviewPolyline { get; set; }
    }

    public class OverviewPolyline
    {
        [JsonProperty("points")]
        public string Points { get; set; }
    }
    public class Leg
    {
        public Value Distance { get; set; }
        public Value Duration { get; set; }

        
        [JsonProperty("start_location")]
        public Location StartLocation { get; set; }

        [JsonProperty("end_location")]
        public Location EndLocation { get; set; }

        [JsonProperty("start_address")]
        public string StartAddress { get; set; }

        [JsonProperty("end_address")]
        public string EndAddress { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }
    }

    public class Step
    {
        public Value Distance { get; set; }
        public Value Duration { get; set; }
        [JsonProperty("start_location")]

        public Location StartLocation { get; set; }
        [JsonProperty("end_location")]
        public Location EndLocation { get; set; }

    }

    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}
