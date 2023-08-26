using LincCut.Dto;
using LincCut.Repository;
using LincCut.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LincCut.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
        private readonly IUserRepository _userRepository;
        private readonly IServiceForAuth _serviceForAuth;
        public AuthController(IUserRepository userRepository, IServiceForAuth serviceForAuth)
        {
            _serviceForAuth = serviceForAuth;
            _userRepository = userRepository;
        }
        [HttpGet("/api/auth/guest")]
        public async Task<ActionResult<UserDTO>> Guest()
        {
            return Ok(await _serviceForAuth.Guest(_userRepository, HttpContext));
        }
        [Authorize]
        [HttpPost("/api/auth/register")]
        public async Task<ActionResult<UserDTO>> Registration(UserDTO userDTO)
        {
            return Ok(await _serviceForAuth.Registration(userDTO, HttpContext, _userRepository, User));
        }
        [HttpPost("/api/auth/login")]
        public async Task<ActionResult<UserDTO>> LogIn(UserDTO userDTO)
        {
            return Ok(await _serviceForAuth.LogIn(userDTO, _userRepository, HttpContext));
        }
        [Authorize]
        [HttpPost("/api/auth/logout")]
        public async Task<IActionResult> LogOut()
        {
            return Ok(await _serviceForAuth.LogOut(HttpContext));
        }
    }
}
