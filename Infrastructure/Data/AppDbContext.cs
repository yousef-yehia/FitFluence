using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FavouriteFood> FavouriteFoods { get; set; }
        public DbSet<UserGoals> UserGoals { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }

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
            modelBuilder.Entity<FavouriteFood>()
                 .HasKey(uf => new { uf.AppUserId, uf.FoodId });

            modelBuilder.Entity<FavouriteFood>()
                .HasOne(uf => uf.AppUser)
                .WithMany(u => u.FavouriteFoods)
                .HasForeignKey(uf => uf.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavouriteFood>()
                .HasOne(uf => uf.Food)
                .WithMany(f => f.UserFoods)
                .HasForeignKey(uf => uf.FoodId)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure the one-to-many relationship between muscle and exercises
            modelBuilder.Entity<Muscle>()
                .HasMany(m => m.Exercises)
                .WithOne(e => e.Muscle)
                .HasForeignKey(e => e.MuscleId);

            // Configure the one-to-many relationship between user and WorkoutPlan 
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(m => m.AppUser)
                .WithMany(e => e.WorkoutPlans)
                .HasForeignKey(e => e.AppUserId);

            // Configure the many-to-many relationship between user and favourite foods
            modelBuilder.Entity<WorkoutPlanExercise>()
                 .HasKey(uf => new { uf.WorkoutPlanId, uf.ExerciseId });

            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(uf => uf.WorkoutPlan)
                .WithMany(u => u.WorkOutPlanExercises)
                .HasForeignKey(uf => uf.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(uf => uf.Exercise)
                .WithMany(f => f.workoutPlanExercises)
                .HasForeignKey(uf => uf.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<AppUser>().Ignore(c => c.LockoutEnabled).Ignore(c => c.TwoFactorEnabled);

            //modelBuilder.Entity<AppUser>().ToTable("Users"); // Change users table name
            //modelBuilder.Entity<IdentityRole>().ToTable("Roles"); // Change roles table name
            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims"); // Change user claims table name
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles"); // Change user roles table name
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins"); // Change user logins table name
            //modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens"); // Change user tokens table name
            //modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims"); // Change role claims table name
        }

    }
}
