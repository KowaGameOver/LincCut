using AutoMapper;
using LincCut.AppSettings;
using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Models;
using LincCut.Repository;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Wangkanai.Detection.Services;

namespace LincCut.ServiceLayer
{
    public class ServiceForShortCut : IServiceForShortCut
    {
        private readonly IMapper _mapper;
        private readonly IOptions<HostName> hostName;
        private readonly IOptions<Alphabet> alphabet;
        private readonly IDetectionService _detectionService;
        private readonly Click newClick;
        private readonly Url newUrl;
        private UrlInfoDto newUrlDto;
        private int counterIsInfinity = 0;
        private const string strRegex = @"((http|https)://)(www.)?" +
      "[a-zA-Z0-9@:%._\\+~#?&//=]" +
      "{2,256}\\.[a-z]" +
      "{2,6}\\b([-a-zA-Z0-9@:%" +
      "._\\+~#?&//=]*)";
        Regex re = new Regex(strRegex);

        public ServiceForShortCut(IDetectionService detectionService, IOptions<HostName> hostName, IOptions<Alphabet> alphabet, IMapper mapper)
        {
            _mapper = mapper;
            _detectionService = detectionService;
            newClick = new();
            newUrl = new();
            this.hostName = hostName;
            this.alphabet = alphabet;
        }
        public async Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls, UrlInfoAddDTO urlInfoAddDTO, DateTime dateForExpire, ClaimsPrincipal claimsPrincipal)
        {
            if (string.IsNullOrEmpty(urlInfoAddDTO.ORIGINAL_URL))
                throw new BadHttpRequestException("Url must be entered");
            if (urlInfoAddDTO.ORIGINAL_URL.Contains(hostName.Value.hostName))
                throw new BadHttpRequestException("You can`t make short link for this site!");
            if (repositoryForUrls == null)
                throw new BadHttpRequestException("Urls repository is null");
            if (re.IsMatch(urlInfoAddDTO.ORIGINAL_URL))
            {
                StringBuilder sb = new StringBuilder();
                newUrl.ORIGINAL_URL = urlInfoAddDTO.ORIGINAL_URL;
                newUrl.CREATED_AT = DateTime.Now;
                newUrl.USER_ID = int.Parse(claimsPrincipal.Claims.First(i => i.Type == "id").Value);
                if (dateForExpire != DateTime.MinValue)
                {
                    newUrl.EXPIRED_AT = dateForExpire;
                }
                await AddCounter(newUrl, urlInfoAddDTO.MAX_CLICKS);
                await repositoryForUrls.CreateAsync(newUrl);
                var i = newUrl.ID;
                sb = await GenerateShortCut(sb, i);
                newUrl.SHORT_SLUG = sb.ToString();
                await repositoryForUrls.UpdateAsync(newUrl);
                newUrlDto = _mapper.Map<UrlInfoDto>(newUrl);
                newUrlDto.SHORT_SLUG = string.Concat(hostName.Value.hostName, newUrl.SHORT_SLUG);
            }
            else
                throw new BadHttpRequestException("Invalid reference!");
            return newUrlDto;
        }
        public async Task<string> OkRedirectResultAsync(IUrlInfoRepository repositoryForUrls, IClickRepository repositoryForClicks, string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new BadHttpRequestException("Url must be entered");
            if (repositoryForClicks == null)
                throw new BadHttpRequestException("CLicks repository is null");
            var newUrl = await repositoryForUrls.CheckNewUrlAsync(u => u.SHORT_SLUG == url);
            if (newUrl == null)
                throw new BadHttpRequestException("Invalid url");
            newClick.URL_INFO_ID = newUrl.ID;
            newClick.IP = await GetLocalIPAddress();
            newClick.BROWSER = _detectionService.Browser.Name.ToString();
            await repositoryForClicks.CreateAsync(newClick);
            await CheckExpired(newUrl, repositoryForClicks, repositoryForUrls);
            return newUrl.ORIGINAL_URL;
        }
        public async Task OkDeleteUrlAsync(string url, IUrlInfoRepository repositoryForUrls)
        {
            if (string.IsNullOrEmpty(url))
                throw new BadHttpRequestException("Url must be entered");
            var urlForDelete = await repositoryForUrls.CheckNewUrlAsync(u => u.SHORT_SLUG == url);
            if (urlForDelete == null)
                throw new BadHttpRequestException("Url didn`t found");

            await repositoryForUrls.DeleteUrlAsync(urlForDelete, repositoryForUrls);
        }
        private async Task<StringBuilder> GenerateShortCut(StringBuilder sb, int i)
        {
            do
            {
                sb.Insert(0, alphabet.Value.alphabet[i % 64]);
                i = i / 64;
            }
            while (i != 0);
            return sb;
        }
        private static async Task<string> GetLocalIPAddress()
        {
            IPHostEntry iph;
            string myip = string.Empty;
            iph = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in iph.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myip = ip.ToString();
                }
            }
            return myip;
        }
        private async Task<Url> AddCounter(Url newUrl, int counter)
        {
            if (counter == counterIsInfinity)
                newUrl.MAX_CLICKS = counterIsInfinity;
            if (counter != counterIsInfinity)
                newUrl.MAX_CLICKS = counter;
            return newUrl;
        }
        private async Task CheckExpired(Url newUrl, IClickRepository repositoryForClicks, IUrlInfoRepository repositoryForUrls)
        {
            if (newUrl.EXPIRED_AT <= DateTime.Now && newUrl.EXPIRED_AT != DateTime.MinValue)
                throw new BadHttpRequestException("Reference is expired");
            if (await repositoryForClicks.CheckNewClickAsync(c => c.URL_INFO_ID == newUrl.ID) == newUrl.MAX_CLICKS)
            {
                newUrl.EXPIRED_AT = DateTime.Now;
                await repositoryForUrls.UpdateAsync(newUrl);
            }
        }
    }
}
