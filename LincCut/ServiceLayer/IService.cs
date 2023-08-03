using LincCut.Mocks;
using LincCut.Repository;
using System.Runtime.InteropServices;

namespace LincCut.ServiceLayer
{
    public interface IService
    {
        Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls, string url, [Optional] int counter, [Optional] DateTime date);
        Task<string> OkRedirectResultAsync(IUrlInfoRepository repositoryForUrls, IClickRepository repositoryForClicks, string url);
        Task OkDeleteUrlAsync(string url, IUrlInfoRepository repositoryForUrls);
    }
}
