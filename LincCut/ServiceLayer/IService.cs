using LincCut.Mocks;
using LincCut.Repository;
using System.Runtime.InteropServices;

namespace LincCut.ServiceLayer
{
    public interface IService
    {
        Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repository, string url, [Optional] int counter, [Optional] DateTime date);
        Task<string> OkRedirectResult(IUrlInfoRepository repository, IClickRepository repositoryForClicks, string url);
    }
}
