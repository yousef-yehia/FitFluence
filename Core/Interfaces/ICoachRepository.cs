using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces

{
    public interface ICoachRepository
    {
        public Task<Coach> UpdateAsync(Coach coach);
        public Task CreateAsync(Coach coach);
        public Task<List<Coach>> GetAllUsersAsync(Expression<Func<Coach, bool>>? filter = null, string? includeProperties = null);
        Task<Coach> GetAsync(Expression<Func<Coach, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        public Task DeleteUserAsync(Coach coach);
    }
}
