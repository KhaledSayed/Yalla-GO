using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class ChatTrip
    {
        public int Id { get; set; }

        public Trip Trip { get; set; }

        public int TridId { get; set; }

        public Client Client { get; set; }

        public int ClientId { get; set; }

        public Driver Driver { get; set; }

        public string Message { get; set; }

    }
}
