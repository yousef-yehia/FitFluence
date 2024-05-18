using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Coach> Coachs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<UserFoods> UserFoods { get; set; }
        public DbSet<UserGoals> UserGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the many-to-many relationship between user and goals
            modelBuilder.Entity<UserGoals>()
                .HasKey(ug => new { ug.AppUserId, ug.GoalId });

            modelBuilder.Entity<UserGoals>()
                .HasOne(ug => ug.AppUser)
                .WithMany(u => u.UserGoals)
                .HasForeignKey(ug => ug.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<UserGoals>()
                .HasOne(ug => ug.Goal)
                .WithMany(g => g.UserGoals)
                .HasForeignKey(ug => ug.GoalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the many-to-many relationship between user and favourite foods
            modelBuilder.Entity<UserFoods>()
                 .HasKey(uf => new { uf.AppUserId, uf.FoodId });

            modelBuilder.Entity<UserFoods>()
                .HasOne(uf => uf.AppUser)
                .WithMany(u => u.UserFoods)
                .HasForeignKey(uf => uf.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFoods>()
                .HasOne(uf => uf.Food)
                .WithMany(f => f.UserFoods)
                .HasForeignKey(uf => uf.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the one-to-one relationship between user and coach and client
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Client)
                .WithOne(c => c.AppUser)
                .HasForeignKey<Client>(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
            

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Coach)
                .WithOne(c => c.AppUser)
                .HasForeignKey<Coach>(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<AppUser>().Ignore(c => c.LockoutEnabled).Ignore(c => c.TwoFactorEnabled);

            modelBuilder.Entity<AppUser>().ToTable("Users"); // Change users table name
            modelBuilder.Entity<IdentityRole>().ToTable("Roles"); // Change roles table name
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims"); // Change user claims table name
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles"); // Change user roles table name
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins"); // Change user logins table name
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens"); // Change user tokens table name
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims"); // Change role claims table name
        }

    }
}
