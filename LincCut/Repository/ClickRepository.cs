using LincCut.Data;
using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public class ClickRepository : Repository<Click>, IClickRepository
    {
        private readonly AppDbContext _db;
        public ClickRepository()
        {
            _db = new();
        }
        public int CheckNewClick(Expression<Func<Click, bool>>? filter = null)
        {
            IQueryable<Click> query = _db.Clicks;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Count();
        }
    }
}
