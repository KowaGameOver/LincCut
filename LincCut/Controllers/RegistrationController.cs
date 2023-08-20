using AutoMapper;
using BCrypt.Net;
using LincCut.Data;
using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Models;
using LincCut.test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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
        private readonly ITokenManager _tokenManager;
        public RegistrationController(IConfiguration configuration, AppDbContext db, IMapper mapper, ITokenManager tokenManager)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
            _tokenManager = tokenManager;
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
                _db.users.Update(newUser);
                _db.SaveChanges();
            }
            else
            {
                _db.users.Add(newUser);
                _db.SaveChanges();
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
            {
                throw new BadHttpRequestException("Invalid login.");
            }
            if (loginUser.Password != passForCheck)
            {
                throw new BadHttpRequestException("Invalid password.");
            }
            loginUser.LastLogin = DateTime.Now;
            _db.users.Update(loginUser);
            _db.SaveChanges();
            userDTO = _mapper.Map<UserDTO>(loginUser);
            userDTO.Token = CreateToken(userDTO);
            return Ok(userDTO);
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult GetUsers()
        {
            return Ok(_db.users);
        }

        [HttpPost("tokens/cancel")]
        public async Task<IActionResult> CancelAccessToken()
        {
            await _tokenManager.DeactivateCurrentAsync();

            return NoContent();
        }
        [Authorize]
        [HttpPost("/api/auth/logout")]
        public ActionResult<string> LogOut()
        {
            List<Claim> claims = new List<Claim>(User.Claims);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now,
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string CreateToken(UserDTO userDTO)
        {
            List<Claim> claims = new List<Claim>();
            if (userDTO.Email == null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userDTO.Role.ToString()));
                claims.Add(new Claim("id", userDTO.Id.ToString()));
            }
            if (userDTO.Email != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userDTO.Role.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, userDTO.Email));
                claims.Add(new Claim("id", userDTO.Id.ToString()));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(365),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        } 
    }
}
