using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitFluence.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserRepository _userRepository;

        public ClientRepository(AppDbContext appDbContext, IUserRepository userRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
        }

        public async Task CreateAsync(Client client)
        {
            await _appDbContext.Clients.AddAsync(client);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Client client)
        {
            _appDbContext.Clients.Remove(client);
            await _appDbContext.SaveChangesAsync();
            var user = await _userRepository.GetAsync(u => u.Id == client.AppUserId);
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<List<Client>> GetAllClientsAsync(Expression<Func<Client, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Client> usersQuery = _appDbContext.Clients.AsQueryable();
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

        public async Task<Client> GetAsync(Expression<Func<Client, bool>> filter = null, bool tracked = true, string includeProperties = null)
        {
            IQueryable<Client> usersQuery = _appDbContext.Clients.AsQueryable();

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

        public async Task<int> GetClientIdFromAppUserIdAsync(string appUserId)
        {
            var appUser = await _appDbContext.Users.Include(u=>u.Client).FirstOrDefaultAsync(u=>u.Id==appUserId);
            return appUser.Client.ClientId;
        }

        public async Task<Client> UpdateAsync(Client client)
        {
            _appDbContext.Clients.Update(client);
            await _appDbContext.SaveChangesAsync();
            var user = await _userRepository.GetAsync(u => u.Id == client.AppUserId);
            await _userRepository.UpdateAsync(user);
            return client;
        }
    }
}
