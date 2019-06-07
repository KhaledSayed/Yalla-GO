using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarPooling.DTO;
using CarPooling.Models;
using CarPooling.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarPooling.Controllers
{
    [Route("api/driver/auth")]
    public class DriverAuthController : Controller
    {
        
         private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        public DriverAuthController(IAuthRepository authRepository, IConfiguration iConfiguration)
        {
            this._configuration = iConfiguration;
            this._authRepository = authRepository;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DriverForRegisterDto userForRegisterDto)
        {

            //validate
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await _authRepository.UserExisits(userForRegisterDto.Email))
            {
                return BadRequest("Email Already registered");
            }

            var user = new User()
            {
                Email = userForRegisterDto.Email
            };
            //register
            var registeredUser = await _authRepository.Register(user, userForRegisterDto.Password, Models.enums.UserType.Driver);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(DriverForLoginDto userForLoginDto)
        {

            // throw new Exception("Nooooooo");
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }


            var userFromRepo = await _authRepository.Login(userForLoginDto.Email, userForLoginDto.Password,Models.enums.UserType.Driver);


            if (userFromRepo == null) return Unauthorized();


            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtToken:SecretKey"]));

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Email,userFromRepo.Email.ToString()),
                new Claim(ClaimTypes.Role , userFromRepo.Role.ToString())
            };

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);


            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            Response.Headers.Add("X-Auth", tokenHandler.WriteToken(token));
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
    
}
