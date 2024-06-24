using Microsoft.AspNetCore.Identity;
namespace Core.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public double? MuscleWeight { get; set; }
        public double? FatWeight { get; set; }
        public string? ImageUrl { get; set; }
        public Client? Client { get; set; }
        public Coach? Coach { get; set; }
        public List<UserGoals> UserGoals { get; set; }
        public List<FavouriteFood> FavouriteFoods  { get; set; }
        public List<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
