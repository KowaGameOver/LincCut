using LincCut.Mocks;
using LincCut.Repository;
using LincCut.ServiceLayer;
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
        private readonly IService _service;
        public ShortCutController(IUrlInfoRepository repository, IService service, IClickRepository repositoryForClicks)
        {
            _repositoryForUrlInfos = repository;
            _service = service;
            _repositoryForClicks = repositoryForClicks;
        }
        [HttpPost("/api/shorten")]
        public async Task<ActionResult<UrlInfoDto>> AddUrlAsync(string url, [Optional] int counter, [Optional] DateTime date)
        {
            return Ok(await _service.OkAddUrlAsync(_repositoryForUrlInfos, url, counter, date));
        }
        [HttpGet("{url:required}")]
        public async Task<RedirectResult> RedirectResultAsync(string url)
        {
            return Redirect(await _service.OkRedirectResultAsync(_repositoryForUrlInfos,_repositoryForClicks, url));
        }
        [HttpDelete("/api/delete/{url:required}")]
        public async Task DeleteUrlAsync(string url)
        {
            await _service.OkDeleteUrlAsync(url,_repositoryForUrlInfos);
        }
    }
}