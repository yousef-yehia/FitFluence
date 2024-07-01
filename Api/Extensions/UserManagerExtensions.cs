using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
        public static async Task<AppUser> FindByEmailFromClaimsPrincipalWithFoods(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(x => x.FavouriteFoods)
                .Include(x=> x.Ratings)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
        public static async Task<AppUser> FindByEmailFromClaimsPrincipalWithWorkoutHistories(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(x => x.WorkoutHistories)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        } 
        public static async Task<AppUser> FindByEmailFromClaimsPrincipalWithCoach(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(x => x.Coach)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
        public static async Task<AppUser> FindByEmailFromClaimsPrincipalWithDietPlans(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(x => x.DietPlans)
                .ThenInclude(dp => dp.DietPlanFoods)
                .ThenInclude(dpf=> dpf.Food)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }


    }
}