using Microsoft.AspNetCore.Identity;
namespace Core.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Age { get; set; }
        public int? MuscleWeight { get; set; }
        public int? FatWeight { get; set; }
        public string? ImageUrl { get; set; }
        public string? Cv { get; set; }
        public List<UserGoals> UserGoals { get; set; }
        public List<FavouriteFood> FavouriteFoods  { get; set; }
        public List<WorkoutPlan> WorkoutPlans { get; set; }

    }
}
