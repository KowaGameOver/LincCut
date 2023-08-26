using LincCut.Dto;
using LincCut.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LincCut.ServiceLayer
{
    public interface IServiceForAuth
    {
        Task<UserDTO> Guest(IUserRepository userRepository, HttpContext httpContext);
        Task<UserDTO> Registration(UserDTO userDTO, HttpContext httpContext, IUserRepository userRepository, ClaimsPrincipal claimsPrincipal);
        Task<UserDTO> LogIn(UserDTO userDTO, IUserRepository userRepository, HttpContext httpContext);
        Task<string> LogOut(HttpContext httpContext);
    }
}
