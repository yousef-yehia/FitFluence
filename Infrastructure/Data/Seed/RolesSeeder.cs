using Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seed
{
    public class RolesSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Role.roleAdmin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.roleAdmin));
            }

            if (!await _roleManager.RoleExistsAsync(Role.roleClient))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.roleClient));
            }
            if (!await _roleManager.RoleExistsAsync(Role.roleCoach))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.roleCoach));
            }
        }
    }
}
    