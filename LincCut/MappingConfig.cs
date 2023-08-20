using AutoMapper;
using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Models;

namespace LincCut
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<UrlInfo, UrlInfoDto>();
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
