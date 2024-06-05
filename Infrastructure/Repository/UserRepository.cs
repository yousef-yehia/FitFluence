using Azure;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<AppUser> userManager, AppDbContext appDbContext, IMapper mapper)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<AppUser>> GetAllUsersAsync(Expression<Func<AppUser, bool>>? filter = null, Expression<Func<AppUser, string>>? ordering = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
            if (filter != null)
            {
                usersQuery.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                //skip0.take(5)
                //page number- 2     || page size -5
                //skip(5*(1)) take(5)
                usersQuery = usersQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    usersQuery = usersQuery.Include(includeProp);
                }
            }
            if(ordering != null)
            {
                usersQuery = usersQuery.OrderBy(ordering);
            }
            return await usersQuery.ToListAsync();
        }
        public async Task<AppUser> UpdateAsync(AppUser appUser)
        {
            await _userManager.UpdateAsync(appUser);
            await _appDbContext.SaveChangesAsync();
            return appUser;
        }
        public async Task DeleteUserAsync(AppUser appUser)
        {
            await _userManager.DeleteAsync(appUser);
        }

        public async Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();

            if (!tracked)
            {
                usersQuery = usersQuery.AsNoTracking();
            }
            if (filter != null)
            {
                usersQuery = usersQuery.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    usersQuery = usersQuery.Include(includeProp);
                }
            }
            return await usersQuery.FirstOrDefaultAsync();
        }

        public async Task<bool> DoesUserExists(string id)
        {
            var user = await _appDbContext.Users.AnyAsync(x => x.Id == id);
            if (user)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            if( await _userManager.CheckPasswordAsync(user, password))
            {
                return true;
            }
            return false;
        }

        public async Task AddGoalToUserAsync(AppUser user, List<int> goalIds)
        {
            foreach (var goalId in goalIds)
            {
                var goal = await _appDbContext.Goals.FindAsync(goalId);

                if (goal != null)
                {
                    user.UserGoals.Add(new UserGoals { AppUserId = user.Id, GoalId = goalId });
                }
            }

            await _appDbContext.SaveChangesAsync();
        }
        //public async Task AddFavouriteFoodAsync(AppUser user, int foodId)
        //{

        //    var food = await _appDbContext.Foods.FindAsync(foodId);

        //    if (food != null)
        //    {
        //        user.UserFoods.Add(new UserFoods { AppUserId = user.Id, FoodId = foodId });
        //    }


        //    await _appDbContext.SaveChangesAsync();
        //}

        //public async Task DeleteFavouriteFoodAsync(AppUser appUser, Food food)
        //{
        //    var user = await _appDbContext.Users
        //        .Include(u => u.UserFoods)
        //        .ThenInclude(ug => ug.Food)
        //        .FirstOrDefaultAsync(u => u.Id == appUser.Id);

        //    var userFood = user.UserFoods.FirstOrDefault( a => a.AppUserId == user.Id && a.FoodId == food.Id);

        //    user.UserFoods.Remove(userFood);

        //    await _appDbContext.SaveChangesAsync();
        //}

        //public async Task<List<Food>> GetAllFavouriteFoodsAsync(AppUser appUser, int pageSize = 10, int pageNumber = 1)
        //{

        //    var userFoods = await _appDbContext.UserFoods.Where(u => u.AppUserId == appUser.Id).Select(x => x.Food).ToListAsync();
        //    // Apply pagination if needed
        //    if (pageSize > 0)
        //    {
        //        if (pageSize > 100)
        //        {
        //            pageSize = 100;
        //        }
        //        userFoods = userFoods.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        //    }

        //    return userFoods;
        //}

        public async Task DeleteUserGoalAsync(AppUser appUser, Goal goal)
        {
            var user = await _appDbContext.Users
                .Include(u => u.UserGoals)
                .ThenInclude(ug => ug.Goal)
                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            var userGoal = user.UserGoals.FirstOrDefault(a => a.AppUserId == user.Id && a.GoalId == goal.Id);

            user.UserGoals.Remove(userGoal);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Goal>> GetAllUserGoalsAsync(AppUser appUser, int pageSize = 10, int pageNumber = 1)
        {
            var user = await _appDbContext.Users
                .Include(u => u.UserGoals)
                .ThenInclude(ug => ug.Goal)
                .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            var userGoals = user.UserGoals.Select(ug => ug.Goal).ToList();

            // Apply pagination if needed
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                userGoals = userGoals.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }

            var GoalDtoList = _mapper.Map<List<Goal>>(userGoals);

            return GoalDtoList;
        }

        public void Detach(AppUser entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}

