using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Coach> Coachs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<UserFoods> UserFoods { get; set; }
        public DbSet<UserGoals> UserGoals { get; set; }
    }
}
