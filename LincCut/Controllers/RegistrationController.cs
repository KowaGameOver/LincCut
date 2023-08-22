using AutoMapper;
using LincCut.Data;
using LincCut.Dto;
using LincCut.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LincCut.Controllers
{
    [Route("")]
    [ApiController]
    public class RegistrationController : ControllerBase
    { 
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public RegistrationController(IConfiguration configuration, AppDbContext db, IMapper mapper)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
        }
        [HttpGet("/api/auth/guest")]
        public ActionResult<UserDTO> Guest()
        {
            var user = new User();
            user.RegistrationDate = DateTime.Now;
            user.LastLogin = DateTime.Now;
            _db.users.Add(user);
            _db.SaveChanges();
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Token = CreateToken(userDTO);
            return userDTO;
        }
        [Authorize]
        [HttpPost("/api/auth/register")]
        public ActionResult<UserDTO> Registration(UserDTO userDTO)
        {
            int id = int.Parse(User.Claims.First(i => i.Type == "id").Value);
            userDTO.Id = id;
            var passwordHash = HashPassword(userDTO.Password);
            userDTO.Password = passwordHash;
            var newUser = _mapper.Map<User>(userDTO);
            newUser.RegistrationDate = DateTime.Now;
            newUser.LastLogin = DateTime.Now;
            newUser.Role = Roles.user;
            if (_db.users.AsNoTracking().FirstOrDefault(nu => nu.Id == newUser.Id) != null)
            {
                _db.users.Update(newUser);_db.SaveChanges();
            }
            else
            {
                _db.users.Add(newUser);_db.SaveChanges();
            }
            userDTO = _mapper.Map<UserDTO>(newUser);
            userDTO.Token = CreateToken(userDTO);
            return Ok(userDTO);
        }
        [HttpPost("/api/auth/login")]
        public ActionResult<UserDTO> LogIn(UserDTO userDTO)
        {
            var passForCheck = HashPassword(userDTO.Password);
            var loginUser = _db.users.FirstOrDefault(e => e.Email == userDTO.Email);
            if (loginUser == null)
                return NotFound("Incorrect login");

            if (loginUser.Password != passForCheck)
                return NotFound("Incorrect password");

            loginUser.LastLogin = DateTime.Now;
            _db.users.Update(loginUser);
            _db.SaveChanges();
            userDTO = _mapper.Map<UserDTO>(loginUser);
            userDTO.Token = CreateToken(userDTO);
            return Ok(userDTO);
        }
        [Authorize]
        [HttpPost("/api/auth/logout")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("jwtToken");
            return Ok(new { message = "Logged out successfully" });
        }

        private string CreateToken(UserDTO userDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, userDTO.Role.ToString()),
                    new Claim("id", userDTO.Id.ToString()),
                }),

                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            if (userDTO.Email != null)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, userDTO.Email));
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            HttpContext.Response.Cookies.Append("jwtToken", tokenString);
            return tokenString;
        }
        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                builder.Append(hashBytes[i].ToString("x2"));

            return builder.ToString();
        }
    }
}
