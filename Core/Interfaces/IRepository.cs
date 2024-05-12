using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, string>>? ordering = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<bool> DoesExistAsync(Expression<Func<T, bool>> filter = null);

        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();

    }
}
