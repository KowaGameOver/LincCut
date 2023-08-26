using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Repository;
using System.Security.Claims;

namespace LincCut.ServiceLayer
{
    public interface IServiceForShortCut
    {
        Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls, UrlInfoAddDTO urlInfoAddDTO, DateTime dateForExpire, ClaimsPrincipal claimsPrincipal);
        Task<string> OkRedirectResultAsync(IUrlInfoRepository repositoryForUrls, IClickRepository repositoryForClicks, string url);
        Task OkDeleteUrlAsync(string url, IUrlInfoRepository repositoryForUrls);
    }
}
