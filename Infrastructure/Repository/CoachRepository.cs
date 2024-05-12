using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitFluence.Repository
{
    public class CoachRepository : ICoachRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;

        public CoachRepository(AppDbContext appDbContext, IUserRepository userRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
        }

        public async Task CreateAsync(Coach coach)
        {
            await _appDbContext.AddAsync(coach);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Coach coach)
        {
            _appDbContext.Coachs.Remove(coach);
            await _appDbContext.SaveChangesAsync();
            var user = await _userRepository.GetAsync(u => u.Id == coach.AppUserId);
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<List<Coach>> GetAllUsersAsync(Expression<Func<Coach, bool>> filter = null, string includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<Coach> usersQuery = _appDbContext.Coachs.AsQueryable();
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
            return await usersQuery.ToListAsync();
        }

        public async Task<Coach> GetAsync(Expression<Func<Coach, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            IQueryable<Coach> usersQuery = _appDbContext.Coachs.AsQueryable();

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

        public async Task<Coach> UpdateAsync(Coach coach)
        {
            _appDbContext.Coachs.Update(coach);
            await _appDbContext.SaveChangesAsync();
            var user = await _userRepository.GetAsync(u => u.Id == coach.AppUserId);
            await _userRepository.UpdateAsync(user);
            return coach;
        }
    }
}
