using AutoMapper;
using LincCut.AppSettings;
using LincCut.Mocks;
using LincCut.Models;
using LincCut.Repository;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Wangkanai.Detection.Services;

namespace LincCut.ServiceLayer
{
    public class Service : IService
    {
        private readonly IMapper _mapper;
        private readonly IOptions<HostName> hostName;
        private readonly IOptions<Alphabet> alphabet;
        private readonly IDetectionService _detectionService;
        private readonly Click newClick;
        private readonly UrlInfo newUrl;
        private UrlInfoDto newUrlDto;
        private int counterIsInfinity = 0;
        public Service(IDetectionService detectionService, IOptions<HostName> hostName, IOptions<Alphabet> alphabet, IMapper mapper)
        {
            _mapper = mapper;
            _detectionService = detectionService;
            newClick = new();
            newUrl = new();
            this.hostName = hostName;
            this.alphabet = alphabet;
        }
        public async Task<UrlInfoDto> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls,string url, [Optional] int counter, [Optional] DateTime date)
        {
            if (url == null)
            {
                throw new BadHttpRequestException("Url is null");
            }
            if (url.Contains(hostName.Value.hostName))
            {
                throw new BadHttpRequestException("You can`t make short link for this site!");
            }
            if (repositoryForUrls == null)
            {
                throw new BadHttpRequestException("Urls repository is null");
            }
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                StringBuilder sb = new StringBuilder();
                newUrl.Url = url;
                newUrl.Created_at = DateTime.Now;
                if (date != DateTime.MinValue)
                {
                    newUrl.Expired_at = date;
                }
                await AddCounter(newUrl, counter);
                await repositoryForUrls.CreateAsync(newUrl);
                var i = newUrl.Id;
                sb = await GenerateShortCut(sb, i);
                newUrl.NewUrl = sb.ToString();
                await repositoryForUrls.UpdateAsync(newUrl);
                newUrlDto = _mapper.Map<UrlInfoDto>(newUrl);
                newUrlDto.NewUrl = string.Concat(hostName.Value.hostName, newUrl.NewUrl);
            }
            else
            {
                throw new BadHttpRequestException("Invalid reference!");
            }

            return newUrlDto;
        }
        public async Task<string> OkRedirectResult(IUrlInfoRepository repositoryForUrls, IClickRepository repositoryForClicks, string url)
        {
            var newUrl = repositoryForUrls.CheckNewUrl(u => u.NewUrl == url);
            if (newUrl == null)
            {
                throw new BadHttpRequestException("New url is null");
            }
            if (repositoryForClicks == null)
            {
                throw new BadHttpRequestException("CLicks repository is null");
            }
            newClick.UrlInfo_id = newUrl.Id;
            newClick.Ip = await GetLocalIPAddress();
            newClick.Browser = _detectionService.Browser.Name.ToString();
            await repositoryForClicks.CreateAsync(newClick);
            await CheckExpired(newUrl, repositoryForClicks, repositoryForUrls);

            return newUrl.Url;
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
            {
                newUrl.Counter = counterIsInfinity;
            }
            if (counter != counterIsInfinity)
            {
                newUrl.Counter = counter;
            }
            return newUrl;
        }
        private async Task CheckExpired(UrlInfo newUrl, IClickRepository repositoryForClicks, IUrlInfoRepository repositoryForUrls)
        {
            if (newUrl.Expired_at <= DateTime.Now && newUrl.Expired_at != DateTime.MinValue)
            {
                throw new BadHttpRequestException("Reference is expired");
            }
            if (repositoryForClicks.CheckNewClick(c => c.UrlInfo_id == newUrl.Id) == newUrl.Counter)
            {
                newUrl.Expired_at = DateTime.Now;
                await repositoryForUrls.UpdateAsync(newUrl);
            }
        }
    }
}
