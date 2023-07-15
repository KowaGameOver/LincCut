using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public interface IClickRepository : IRepository<Click>
    {
        int CheckNewClick(Expression<Func<Click, bool>>? filter = null);
    }
}
