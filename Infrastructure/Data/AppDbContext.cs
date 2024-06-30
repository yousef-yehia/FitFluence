using Core.Interfaces;
using Core.Models;
using Core.UtilityModels;
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
        public DbSet<WorkoutHistory> WorkoutHistories { get; set; }
        public DbSet<WorkoutHistoryExercise> WorkoutHistoryExercises { get; set; }
        public DbSet<Coach> Coachs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CoachAndClient> CoachsAndClients { get; set; }
        public DbSet<FoodRating> FoodRatings { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<DietPlanFood> DietPlanFoods { get; set; }

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

            // Configure the many-to-many relationship between user and foods to favouriteFoods
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

            // Configure the many-to-many relationship between user and foods to FoodsRating
            modelBuilder.Entity<FoodRating>()
                 .HasKey(uf => new { uf.AppUserId, uf.FoodId });

            modelBuilder.Entity<FoodRating>()
                .HasOne(r => r.AppUser)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FoodRating>()
                .HasOne(r => r.Food)
                .WithMany(f => f.Ratings)
                .HasForeignKey(r => r.FoodId)
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
            

            // Configure the many-to-many relationship between workoutPlan and exercise
            modelBuilder.Entity<WorkoutPlanExercise>()
                 .HasKey(uf => new { uf.WorkoutPlanId, uf.ExerciseId });

            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(uf => uf.WorkoutPlan)
                .WithMany(u => u.WorkOutPlanExercises)
                .HasForeignKey(uf => uf.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(uf => uf.Exercise)
                .WithMany(f => f.WorkoutPlanExercises)
                .HasForeignKey(uf => uf.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
                        
            // Configure the one-to-many relationship between user and WorkoutHistory 
            modelBuilder.Entity<WorkoutHistory>()
                .HasOne(m => m.AppUser)
                .WithMany(e => e.WorkoutHistories)
                .HasForeignKey(e => e.AppUserId);

            // Configure the many-to-many relationship between WorkoutHistory and exercise
            modelBuilder.Entity<WorkoutHistoryExercise>()
                 .HasKey(uf => new { uf.WorkoutHistoryId, uf.ExerciseId });

            modelBuilder.Entity<WorkoutHistoryExercise>()
                .HasOne(uf => uf.WorkoutHistory)
                .WithMany(u => u.WorkoutHistoryExercises)
                .HasForeignKey(uf => uf.WorkoutHistoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutHistoryExercise>()
                .HasOne(uf => uf.Exercise)
                .WithMany(f => f.WorkoutHistoryExercises)
                .HasForeignKey(uf => uf.ExerciseId)
                .OnDelete(DeleteBehavior.NoAction);

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

            // Configure the many-to-many relationship between Coach and Clients
            modelBuilder.Entity<CoachAndClient>()
                 .HasKey(uf => new { uf.ClientId, uf.CoachId });

            modelBuilder.Entity<CoachAndClient>()
                .HasOne(uf => uf.Client)
                .WithMany(u => u.CoachsAndClients)
                .HasForeignKey(uf => uf.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoachAndClient>()
                .HasOne(uf => uf.Coach)
                .WithMany(f => f.CoachsAndClients)
                .HasForeignKey(uf => uf.CoachId)
                .OnDelete(DeleteBehavior.Restrict);


            // Configure the one-to-many relationship between user and DietPlan 
            modelBuilder.Entity<DietPlan>()
                .HasOne(dp => dp.AppUser)
                .WithMany(u => u.DietPlans)
                .HasForeignKey(dp => dp.AppUserId);

            // Configure the many-to-many relationship between DietPlan and Food
            modelBuilder.Entity<DietPlanFood>()
                 .HasKey(dpf => new { dpf.DietPlanId, dpf.FoodId });

            modelBuilder.Entity<DietPlanFood>()
                .HasOne(dpf => dpf.DietPlan)
                .WithMany(dp => dp.DietPlanFoods)
                .HasForeignKey(dpf => dpf.DietPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DietPlanFood>()
                .HasOne(dpf => dpf.Food)
                .WithMany(f => f.DietPlanFoods)
                .HasForeignKey(dpf => dpf.FoodId)
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
