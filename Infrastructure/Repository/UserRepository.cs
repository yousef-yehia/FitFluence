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
using Microsoft.Identity.Client;

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

        public async Task<List<AppUser>> GetAllUsersAsync(Expression<Func<AppUser, bool>> filter = null, Expression<Func<AppUser, string>> ordering = null, string includeProperties = null)
        {
            IQueryable<AppUser> usersQuery = _userManager.Users.AsQueryable();
            if (filter != null)
            {
                usersQuery.Where(filter);
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

        public void Detach(AppUser entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task AddUserDisease(string userId, List<string> diseaseNames )
        {
            foreach (string disease in diseaseNames)
            {
                _appDbContext.UserDiseases.Add(new UserDisease { AppUserId = userId, DiseaseName = disease });
            }
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<List<string>> GetUserDiseases(string userId)
        {
            var userDiseases = await _appDbContext.UserDiseases.Where(x => x.AppUserId == userId).Select(x => x.DiseaseName).ToListAsync();
            return userDiseases;
        }


        public double? CalculateRecommendedCalories(AppUser user)
        {
            double? RC = null;

            if (user.Weight == 0 || user.Height == 0 || user.Weight == null || user.Height == null || user.Age == 0 || user.ActivityLevel == null || user.MainGoal == null || user.Gender == null)
            {
                return RC;
            }

            if (user.Gender == "Male")
            {
                RC = 88.362 + (13.397 * user.Weight) + (4.799 * user.Height) - (5.677 * user.Age);
            }
            else
            {
                RC = 447.593 + (9.247 * user.Weight) + (3.098 * user.Height) - (4.330 * user.Age);
            }

            switch (user.ActivityLevelName)
            {
                case "Not Very Active":
                    RC *= 1.2;
                    break;
                case "Lightly Active":
                    RC *= 1.375;
                    break;
                case "Active":
                    RC *= 1.55;
                    break;
                case "Very Active":
                    RC *= 1.725;
                    break;
                default:
                    throw new ArgumentException("Invalid activity level");
            }

            switch (user.MainGoal)
            {
                case "Maintain Weight":
                    RC *= 1.0;
                    break;
                case "Lose Weight":
                    RC *= 0.8;
                    break;
                case "Gain Weight":
                    RC *= 1.2;
                    break;
            }

            return RC;
        }
    }
}

