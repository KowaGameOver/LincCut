using LincCut.Mocks;
using LincCut.Models;
using LincCut.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text;

namespace LincCut.ServiceLayer
{
    public interface IService
    {
        Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repository, string url, [Optional] int counter, [Optional] DateTime date);
        Task<string> OkRedirectResult(IUrlInfoRepository repository, IClickRepository repositoryForClicks, string url);
    }
}
