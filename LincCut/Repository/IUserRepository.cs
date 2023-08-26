using LincCut.Models;
using System.Linq.Expressions;

namespace LincCut.Repository
{
    public interface IUserRepository : IRepository<User>{
        Task<User> userExist(Expression<Func<User, bool>>? filter = null);
    }
}
