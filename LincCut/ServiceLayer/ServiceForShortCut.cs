using AutoMapper;
using LincCut.AppSettings;
using LincCut.Dto;
using LincCut.Mocks;
using LincCut.Models;
using LincCut.Repository;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
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
        private readonly UrlInfo newUrl;
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
        public async Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls, UrlInfoAddDTO urlInfoAddDTO, DateTime dateForExpire)
        {
            if (string.IsNullOrEmpty(urlInfoAddDTO.Url))
                throw new BadHttpRequestException("Url must be entered");
            if (urlInfoAddDTO.Url.Contains(hostName.Value.hostName))
                throw new BadHttpRequestException("You can`t make short link for this site!");
            if (repositoryForUrls == null)
                throw new BadHttpRequestException("Urls repository is null");
            if (re.IsMatch(urlInfoAddDTO.Url))
            {
                StringBuilder sb = new StringBuilder();
                newUrl.Url = urlInfoAddDTO.Url;
                newUrl.Created_at = DateTime.Now;
                if (dateForExpire != DateTime.MinValue)
                {
                    newUrl.Expired_at = dateForExpire;
                }
                await AddCounter(newUrl, urlInfoAddDTO.Counter);
                await repositoryForUrls.CreateAsync(newUrl);
                var i = newUrl.Id;
                sb = await GenerateShortCut(sb, i);
                newUrl.NewUrl = sb.ToString();
                await repositoryForUrls.UpdateAsync(newUrl);
                newUrlDto = _mapper.Map<UrlInfoDto>(newUrl);
                newUrlDto.NewUrl = string.Concat(hostName.Value.hostName, newUrl.NewUrl);
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
            var newUrl = await repositoryForUrls.CheckNewUrlAsync(u => u.NewUrl == url);
            if (newUrl == null)
                throw new BadHttpRequestException("Invalid url");
            newClick.UrlInfo_id = newUrl.Id;
            newClick.Ip = await GetLocalIPAddress();
            newClick.Browser = _detectionService.Browser.Name.ToString();
            await repositoryForClicks.CreateAsync(newClick);
            await CheckExpired(newUrl, repositoryForClicks, repositoryForUrls);
            return newUrl.Url;
        }
        public async Task OkDeleteUrlAsync(string url, IUrlInfoRepository repositoryForUrls)
        {
            if (string.IsNullOrEmpty(url))
                throw new BadHttpRequestException("Url must be entered");
            var urlForDelete = await repositoryForUrls.CheckNewUrlAsync(u => u.NewUrl == url);
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
        private async Task<UrlInfo> AddCounter(UrlInfo newUrl, int counter)
        {
            if (counter == counterIsInfinity)
                newUrl.Counter = counterIsInfinity;
            if (counter != counterIsInfinity)
                newUrl.Counter = counter;
            return newUrl;
        }
        private async Task CheckExpired(UrlInfo newUrl, IClickRepository repositoryForClicks, IUrlInfoRepository repositoryForUrls)
        {
            if (newUrl.Expired_at <= DateTime.Now && newUrl.Expired_at != DateTime.MinValue)
                throw new BadHttpRequestException("Reference is expired");
            if (await repositoryForClicks.CheckNewClickAsync(c => c.UrlInfo_id == newUrl.Id) == newUrl.Counter)
            {
                newUrl.Expired_at = DateTime.Now;
                await repositoryForUrls.UpdateAsync(newUrl);
            }
        }
    }
}
