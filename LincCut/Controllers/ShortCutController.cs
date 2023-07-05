using LincCut.Models;
using LincCut.Repository;
using LincCut.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace LincCut.Controllers
{
    [ApiController]
    [Route("")]
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
        [HttpPost(Name = "/AddUrl")]
        public async Task<ActionResult<UrlInfo>> AddUrlAsync(string url)
        {
            return Ok(await _service.OkAddUrlAsync(_repositoryForUrlInfos, url, _repositoryForClicks));
        }
        [HttpGet("{url:alpha}")]
        public async Task<RedirectResult> RedirectResult(string url, [Optional]int counter)
        {
            return Redirect(await _service.OkRedirectResult(_repositoryForUrlInfos, url, counter));
        }
    }
}