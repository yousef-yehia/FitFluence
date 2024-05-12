using Core.Models;

namespace Core.Interfaces
{
    public interface IGoalRepository : IRepository<Goal>
    {
        public Task<Goal> UpdateAsync(Goal goal);

    }
}
