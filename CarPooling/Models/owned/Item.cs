using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models.owned
{
    [Owned]
    public class Item
    {
        public string Text { get; set; }

        public double Value { get; set; }
    }
}
