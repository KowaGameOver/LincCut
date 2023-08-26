using LincCut.Data;
using LincCut.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<User> userExist(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = _db.users.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
