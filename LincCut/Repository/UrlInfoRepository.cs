using LincCut.Data;
using LincCut.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<UrlInfo> CheckNewUrlAsync(Expression<Func<UrlInfo, bool>>? filter = null)
        {
            IQueryable<UrlInfo> query = _db.urls;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task DeleteUrlAsync(UrlInfo url, IUrlInfoRepository urlInfoRepository)
        {
            url.EXPIRED_AT = DateTime.Now;
            await urlInfoRepository.UpdateAsync(url);
        }
    }
}

