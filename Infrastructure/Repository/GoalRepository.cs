//using Core.Interfaces;
//using Core.Models;
//using Infrastructure.Data;
//using Infrastructure.Repository;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructures.Repository
//{
//    public class GoalRepository : Repository<Goal>, IGoalRepository
//    {
//        private readonly AppDbContext _appDbContext;
//        public GoalRepository(AppDbContext appDb, AppDbContext appDbContext) : base(appDb)
//        {
//            _appDbContext = appDbContext;
//        }

//        public async Task<Goal> UpdateAsync(Goal goal)
//        {
//            _appDbContext.Update(goal);
//            await _appDbContext.SaveChangesAsync();
//            return goal;
//        }
//        public async Task AddGoalToUserAsync(AppUser user, List<int> goalIds)
//        {
//            foreach (var goalId in goalIds)
//            {
//                var goal = await _appDbContext.Goals.FindAsync(goalId);

//                if (goal != null)
//                {
//                    user.UserGoals.Add(new UserGoals { AppUserId = user.Id, GoalId = goalId });
//                }
//            }

//            await _appDbContext.SaveChangesAsync();
//        }

//        public async Task DeleteUserGoalAsync(AppUser appUser, Goal goal)
//        {
//            var user = await _appDbContext.Users
//                .Include(u => u.UserGoals)
//                .ThenInclude(ug => ug.Goal)
//                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

//            var userGoal = user.UserGoals.FirstOrDefault(a => a.AppUserId == user.Id);

//            user.UserGoals.Remove(userGoal);

//            await _appDbContext.SaveChangesAsync();
//        }

//        public async Task<List<Goal>> GetAllUserGoalsAsync(AppUser appUser)
//        {
//            var user = await _appDbContext.Users
//                .Include(u => u.UserGoals)
//                .ThenInclude(ug => ug.Goal)
//                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

//            var userGoals = user.UserGoals.Select(ug => ug.Goal).ToList();

//            return userGoals;
//        }

//    }
//}
