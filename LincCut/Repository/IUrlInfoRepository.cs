using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public interface IUrlInfoRepository : IRepository<UrlInfo>
    {
        Task<UrlInfo> CheckNewUrlAsync(Expression<Func<UrlInfo, bool>>? filter = null);
        Task DeleteUrlAsync(UrlInfo url, IUrlInfoRepository urlInfoRepository);
    }
}
