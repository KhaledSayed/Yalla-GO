using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class DistanceMatrix
    {

        [JsonProperty("destination_addresses")]
        public List<String> DestinationAddresses { get; set; }

        [JsonProperty("origin_addresses")]
        public List<string> OriginAddresses { get; set; }


        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }


    }
}
