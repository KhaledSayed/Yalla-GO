using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class Row
    {
        [JsonProperty("elements")]
        public Element[] Elements { get; set; }
    }
}
