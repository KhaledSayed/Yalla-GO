using CarPooling.Models;
using CarPooling.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Repository
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string Password,UserType type);
        Task<User> Login(string Email, string Password, UserType type);

        Task<bool> UserExisits(string Email);
    }
}
