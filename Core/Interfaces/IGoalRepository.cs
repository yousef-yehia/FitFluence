using Core.Models;

namespace Core.Interfaces
{
    public interface IGoalRepository : IRepository<Goal>
    {
        public Task<Goal> UpdateAsync(Goal goal);
        public Task AddGoalToUserAsync(AppUser user, List<int> goalIds);
        public Task<List<Goal>> GetAllUserGoalsAsync(AppUser appUser);
        public Task DeleteUserGoalAsync(AppUser appUser, Goal goal);

    }
}
