using AutoMapper;
using LincCut.Dto;
using LincCut.Models;
using LincCut.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LincCut.ServiceLayer
{
    public class ServiceForAuth : IServiceForAuth
    {
        private readonly IMapper _mapper; 
        private readonly IConfiguration _configuration;
        public ServiceForAuth(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<UserDTO> Guest(IUserRepository userRepository, HttpContext httpContext)
        {
            var user = new User();
            user.REGISTRATION_DATE = DateTime.Now;
            user.LAST_LOGIN_DATE = DateTime.Now;
            await userRepository.CreateAsync(user);
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.TOKEN = await CreateToken(userDTO, httpContext);
            return userDTO;
        }
        public async Task<UserDTO> LogIn(UserDTO userDTO, IUserRepository userRepository, HttpContext httpContext)
        {
            var passForCheck = await HashPassword(userDTO.PASSWORD);
            var loginUser = await userRepository.userExist(e => e.EMAIL == userDTO.EMAIL);
            if (loginUser == null)
                throw new BadHttpRequestException("Incorrect login");

            if (loginUser.PASSWORD != passForCheck)
                throw new BadHttpRequestException("Incorrect password");

            loginUser.LAST_LOGIN_DATE = DateTime.Now;
            await userRepository.UpdateAsync(loginUser);
            userDTO = _mapper.Map<UserDTO>(loginUser);
            userDTO.TOKEN = await CreateToken(userDTO, httpContext);
            return userDTO;
        }
        public async Task<string> LogOut(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("jwtToken");
            return "Logout succsessfully";
        }
        public async Task<UserDTO> Registration(UserDTO userDTO, HttpContext httpContext, IUserRepository userRepository, ClaimsPrincipal claimsPrincipal)
        {
            int id = int.Parse(claimsPrincipal.Claims.First(i => i.Type == "id").Value);
            userDTO.ID = id;
            var passwordHash = await HashPassword(userDTO.PASSWORD);
            userDTO.PASSWORD = passwordHash;
            var newUser = _mapper.Map<User>(userDTO);
            newUser.REGISTRATION_DATE = DateTime.Now;
            newUser.LAST_LOGIN_DATE = DateTime.Now;
            newUser.ROLE = Roles.user;
            if (await userRepository.userExist(nu => nu.ID == newUser.ID) != null)
                await userRepository.UpdateAsync(newUser);
            else
                await userRepository.CreateAsync(newUser);
            userDTO = _mapper.Map<UserDTO>(newUser);
            userDTO.TOKEN = await CreateToken(userDTO, httpContext);
            return userDTO;
        }
        private async Task<string> CreateToken(UserDTO userDTO,HttpContext httpContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, userDTO.ROLE.ToString()),
                    new Claim("id", userDTO.ID.ToString()),
                }),

                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            if (userDTO.EMAIL != null)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, userDTO.EMAIL));
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            httpContext.Response.Cookies.Append("jwtToken", tokenString);
            return tokenString;
        }
        private async Task<string> HashPassword(string password)
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
