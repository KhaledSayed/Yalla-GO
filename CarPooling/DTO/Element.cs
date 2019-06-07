using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.DTO
{
    public class Element
    {
        public string Status { get; set; }
        public Value Duration { get; set; }
        public Value Distance { get; set; }
    }
}
