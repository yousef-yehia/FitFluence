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
    }
}

