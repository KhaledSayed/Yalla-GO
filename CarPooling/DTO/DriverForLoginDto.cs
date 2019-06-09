using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarPooling.DTO
{
    public class DriverForLoginDto 
    {
        public string Email { get; set; }

        public string Password { get; set; }

    }
}
