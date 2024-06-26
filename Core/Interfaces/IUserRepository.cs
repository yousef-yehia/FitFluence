﻿using System.Linq.Expressions;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<AppUser> UpdateAsync(AppUser appUser);
        public Task<List<AppUser>> GetAllUsersAsync(Expression<Func<AppUser, bool>>? filter = null, Expression<Func<AppUser, string>>? ordering = null, string? includeProperties = null);
        Task<AppUser> GetAsync(Expression<Func<AppUser, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        public Task<bool> CheckPasswordAsync(AppUser user, string password);
        public Task<bool> DoesUserExists(string id);
        //public void Detach(AppUser entity);
        public Task DeleteUserAsync(AppUser user);

    }
}
