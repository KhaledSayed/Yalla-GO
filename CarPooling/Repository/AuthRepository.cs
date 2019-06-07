using CarPooling.Data;
using CarPooling.Models;
using CarPooling.Models.enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext Context)
        {
            this._context = Context;

        }
        public async Task<User> Login(string Email, string Password,UserType type)
        {
            User user  = null;
            if (type == UserType.Driver)
            {
                user = await _context.Drivers.FirstOrDefaultAsync(x => x.Email == Email);
            }
            else
            {
                user = await _context.Clients.FirstOrDefaultAsync(x => x.Email == Email);
            }

            if (user == null)
                return null;

            if (!VerifyHashPassword(Password, user.HashPassword, user.SaltPassword))
                return null;

            return user;
        }

        private bool VerifyHashPassword(string password, byte[] hashPassword, byte[] saltPassword)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(saltPassword))
            {
                var computedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedPassword.Length; i++)
                {
                    if (computedPassword[i] != hashPassword[i]) return false;
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string Password, UserType type)
        {
            Driver driver = null;
            Client client = null;

            byte[] PasswordHash, PasswordSalt;
            CreateHashPassword(Password, out PasswordHash, out PasswordSalt);
            user.HashPassword = PasswordHash;
            user.SaltPassword = PasswordSalt;

            if(type == UserType.Driver)
            {
                driver = new Driver()
                {
                    Email = user.Email,
                    SaltPassword = PasswordSalt,
                    HashPassword = PasswordHash,
                    Role = UserType.Driver.ToString(),
                    Name = user.Name
                };
                await _context.Drivers.AddAsync(driver);
            }
            else
            {
                client = new Client()
                {
                    Email = user.Email,
                    SaltPassword = PasswordSalt,
                    HashPassword = PasswordHash,
                    Role = UserType.Client.ToString(),
                    Name = user.Name
                };
                await _context.Clients.AddAsync(client);
            }

            await _context.SaveChangesAsync();

            return user;
        }

        private void CreateHashPassword(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }

        }

        public async Task<bool> UserExisits(string Email)
        {
            if (!await _context.Users.AnyAsync(u => u.Email == Email)) return false;

            return true;
        }
    }
}
