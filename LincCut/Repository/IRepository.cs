using LincCut.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text;

namespace LincCut.Repository
{
    public interface IRepository<T> where T : class
    {
        Task UpdateAsync(T entity);
        Task CreateAsync(T entity);
        Task SaveAsync();
    }
}
