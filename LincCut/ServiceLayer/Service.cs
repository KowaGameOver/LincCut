using LincCut.Models;
using LincCut.Repository;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Wangkanai.Detection.Services;

namespace LincCut.ServiceLayer
{
    public class Service : IService
    {
        private const string alphabet = "ELuPNUJqxdViYhjT8H-kt3MzFwmO0AyvDK2bSlrInBos9ef_5Q16ZCaXGcRWg7p4";
        private readonly IDetectionService _detectionService;
        private readonly Click newClick;
        private readonly UrlInfo newUrl;
        private int counterIsOver = -1;
        private int counterIsInfinity = 0;
        public Service(IDetectionService detectionService)
        {
            _detectionService = detectionService;
            newClick = new();
            newUrl = new();
        }
        public async Task<UrlInfo> OkAddUrlAsync(IUrlInfoRepository repositoryForUrls,string url, IClickRepository repositoryForClicks, [Optional] int counter)
        {
            if (url == null)
            {
                throw new NullReferenceException();//ADD EXCEPTION filtering!
            }
            if (repositoryForUrls == null)
            {
                throw new NullReferenceException();
            }
            if (repositoryForClicks == null)
            {
                throw new NullReferenceException();
            }

            StringBuilder sb = new StringBuilder();
            newUrl.Url = url;
            if (counter == counterIsInfinity)
            {
                newUrl.Counter = counterIsInfinity;
            }
            if(counter != counterIsInfinity)
            {
                newUrl.Counter = counter;
            }
            await repositoryForUrls.CreateAsync(newUrl);
            var i = newUrl.Id;
            sb = await GenerateShortCut(sb, i);
            newUrl.NewUrl = sb.ToString();
            await repositoryForUrls.UpdateAsync(newUrl);

            newClick.UrlInfo_id = newUrl.Id;
            newClick.Ip = await GetLocalIPAddress();
            newClick.Browser = _detectionService.Browser.Name.ToString();
            await repositoryForClicks.CreateAsync(newClick);
            return newUrl;
        }
        public async Task<string> OkRedirectResult(IUrlInfoRepository repositoryForUrls, string url)
        {
            var newUrl = repositoryForUrls.CheckNewUrl(u => u.NewUrl == url);
            if (newUrl == null)
            {
                throw new NullReferenceException();
            }
            if (newUrl.Counter == counterIsOver)
            {
                throw new BadHttpRequestException("Counter is over! -> 404");
            }
            if (newUrl.Counter != counterIsInfinity && newUrl.Counter != counterIsOver)
            {
                newUrl.Counter--;
                await repositoryForUrls.UpdateAsync(newUrl);
                if (newUrl.Counter == counterIsInfinity)
                {
                    newUrl.Counter = counterIsOver;
                    await repositoryForUrls.UpdateAsync(newUrl);
                }
            }
            return newUrl.Url;
        }
        private static async Task<StringBuilder> GenerateShortCut(StringBuilder sb, int i)
        {
            do
            {
                sb.Insert(0, alphabet[i % 64]);
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
    }
}
