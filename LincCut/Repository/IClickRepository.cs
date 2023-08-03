using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public interface IClickRepository : IRepository<Click>
    {
        Task<int> CheckNewClickAsync(Expression<Func<Click, bool>>? filter = null);
    }
}
