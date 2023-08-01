namespace LincCut.Repository
{
    public interface IRepository<T> where T : class
    {
        Task UpdateAsync(T entity);
        Task CreateAsync(T entity);
        Task SaveAsync();
    }
}
