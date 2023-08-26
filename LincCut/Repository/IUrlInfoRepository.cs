using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public interface IUrlInfoRepository : IRepository<Url>
    {
        Task<Url> CheckNewUrlAsync(Expression<Func<Url, bool>>? filter = null);
        Task DeleteUrlAsync(Url url, IUrlInfoRepository urlInfoRepository);
    }
}
