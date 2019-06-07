using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class Value
    {

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int CurrentValue { get; set; }
    }
}
