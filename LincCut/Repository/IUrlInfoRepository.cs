using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public interface IUrlInfoRepository : IRepository<UrlInfo>
    {
        UrlInfo CheckNewUrl(Expression<Func<UrlInfo, bool>>? filter = null);
    }
}
