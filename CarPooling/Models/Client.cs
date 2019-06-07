using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class Client : User
    {

        public ICollection<Trip> Trips { get; set; }

        public bool Activated { get; set; }


        public Client()
        {
            Trips = new Collection<Trip>();

        }
    }
}
