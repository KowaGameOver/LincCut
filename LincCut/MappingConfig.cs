using AutoMapper;
using LincCut.Mocks;
using LincCut.Models;

namespace LincCut
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<UrlInfo, UrlInfoDto>();
        }
    }
}
