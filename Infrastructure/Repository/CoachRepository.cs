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

        public async Task AddClientToCoachAsync(int clientId, int coachId)
        {
            await _appDbContext.CoachsAndClients.AddAsync(new CoachAndClient { CoachId = coachId, ClientId = clientId });
            await _appDbContext.SaveChangesAsync();
        }
        public async Task RemoveClientFromCoachAsync(int clientId, int coachId)
        {
            _appDbContext.CoachsAndClients.Remove(new CoachAndClient { CoachId = coachId, ClientId = clientId });
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> ClientExistInCoachClientsAsync(int coachId, int clientId)
        {
            return await _appDbContext.CoachsAndClients.AnyAsync(c => c.CoachId == coachId && c.ClientId == clientId);
        }

        public async Task CreateAsync(Coach coach)
        {
            await _appDbContext.AddAsync(coach);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteCoachAsync(Coach coach)
        {
            _appDbContext.Coachs.Remove(coach);
            await _appDbContext.SaveChangesAsync();
            var user = await _userRepository.GetAsync(u => u.Id == coach.AppUserId);
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<List<Client>> GetAllCoachClientsAsync(int coachId)
        {
            var clients = await _appDbContext.Clients.Include(c=> c.AppUser).Where(c => c.CoachsAndClients.Any(cac => cac.CoachId == coachId)).ToListAsync();
            return clients;
        }

        public async Task<List<Coach>> GetAllCoachsAsync(Expression<Func<Coach, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Coach> usersQuery = _appDbContext.Coachs.AsQueryable();

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
