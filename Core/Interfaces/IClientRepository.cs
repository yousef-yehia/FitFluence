﻿using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IClientRepository
    {
        public Task<Client> UpdateAsync(Client client);
        public Task CreateAsync(Client client);
        public Task<List<Client>> GetAllUsersAsync(Expression<Func<Client, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
        Task<Client> GetAsync(Expression<Func<Client, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        public Task DeleteUserAsync(Client client);
    }
}
