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
                .Include(x => x.UserFoods)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}