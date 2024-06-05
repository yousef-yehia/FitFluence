using System.Linq.Expressions;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<AppUser> UpdateAsync(AppUser appUser);
        public Task<List<AppUser>> GetAllUsersAsync(Expression<Func<AppUser, bool>>? filter = null, Expression<Func<AppUser, string>>? ordering = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
        Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        public Task<bool> CheckPasswordAsync(AppUser user, string password);
        public Task<bool> DoesUserExists(string id);
        //public void Detach(AppUser entity);
        public Task DeleteUserAsync(AppUser user);
        public Task AddGoalToUserAsync(AppUser user, List<int> goalIds);
        //public Task DeleteFavouriteFoodAsync(AppUser user, Food food);
        public Task DeleteUserGoalAsync(AppUser user, Goal goal);
        //public Task AddFavouriteFoodAsync(AppUser user, int foodId);
        //public Task<List<Food>> GetAllFavouriteFoodsAsync(AppUser appUser, int pageSize = 10, int pageNumber = 1);
        public Task<List<Goal>> GetAllUserGoalsAsync(AppUser appUser, int pageSize = 10, int pageNumber = 1);

    }
}
