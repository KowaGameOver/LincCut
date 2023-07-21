using LincCut.Data;
using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public class UrlInfoRepository : Repository<UrlInfo>, IUrlInfoRepository
    {
        private readonly AppDbContext _db;
        public UrlInfoRepository(AppDbContext db):base(db)
        {
            _db = db;
        }
        public UrlInfo CheckNewUrl(Expression<Func<UrlInfo, bool>>? filter = null)
        {
            IQueryable<UrlInfo> query = _db.urls;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }
    }
}

