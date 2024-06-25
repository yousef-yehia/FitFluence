using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces

{
    public interface ICoachRepository
    {
        public Task<Coach> UpdateAsync(Coach coach);
        public Task CreateAsync(Coach coach);
        public Task<List<Coach>> GetAllCoachsAsync(Expression<Func<Coach, bool>>? filter = null, string? includeProperties = null);
        Task<Coach> GetAsync(Expression<Func<Coach, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        public Task DeleteCoachAsync(Coach coach);
        public Task AddClientToCoachAsync(int coachId, int clientId);
        public Task<bool> ClientExistInCoachClientsAsync(int coachId, int clientId);
    }
}
