using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Repository;
using System.Runtime.InteropServices;

namespace LincCut.ServiceLayer
{
    public interface IServiceForShortCut
    {
        Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls, UrlInfoAddDTO urlInfoAddDTO, DateTime dateForExpire);
        Task<string> OkRedirectResultAsync(IUrlInfoRepository repositoryForUrls, IClickRepository repositoryForClicks, string url);
        Task OkDeleteUrlAsync(string url, IUrlInfoRepository repositoryForUrls);
    }
}
