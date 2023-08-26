using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Models;
using LincCut.Repository;
using LincCut.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace LincCut.Controllers
{
    [Route("")]
    [ApiController]
    public class ShortCutController : ControllerBase
    {
        private readonly IUrlInfoRepository _repositoryForUrlInfos;
        private readonly IClickRepository _repositoryForClicks;
        private readonly IServiceForShortCut _service;
        public ShortCutController(IUrlInfoRepository repository, IServiceForShortCut service, IClickRepository repositoryForClicks)
        {
            _repositoryForUrlInfos = repository;
            _service = service;
            _repositoryForClicks = repositoryForClicks;
        }
        [Authorize]
        [HttpPost("/api/shorten")]
        public async Task<ActionResult<UrlInfoDto>> AddUrlAsync(UrlInfoAddDTO urlInfoAddDTO, [Optional]DateTime dateForExpire)
        {
            return Ok(await _service.OkAddUrlAsync(_repositoryForUrlInfos, urlInfoAddDTO, dateForExpire, User));
        }
        [Authorize]
        [HttpGet("{url:required}")]
        public async Task<RedirectResult> RedirectResultAsync(string url)
        {
            return Redirect(await _service.OkRedirectResultAsync(_repositoryForUrlInfos,_repositoryForClicks, url));
        }
        [Authorize]
        [HttpDelete("/api/delete")]
        public async Task DeleteUrlAsync(string url)
        {
            await _service.OkDeleteUrlAsync(url,_repositoryForUrlInfos);
        }
    }
}