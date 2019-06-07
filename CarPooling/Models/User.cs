using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Avatar { get; set; }

        public byte[] SaltPassword { get; set; }

        public byte[] HashPassword { get; set; }

        public string Role { get; set; }

    }
}
