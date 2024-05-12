using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructures.Repository
{
    public class GoalRepository : Repository<Goal>, IGoalRepository
    {
        private readonly AppDbContext _appDbContext;
        public GoalRepository(AppDbContext appDb, AppDbContext appDbContext) : base(appDb)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Goal> UpdateAsync(Goal goal)
        {
            _appDbContext.Update(goal);
            await _appDbContext.SaveChangesAsync();
            return goal;
        }
    }
}
