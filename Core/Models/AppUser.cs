using Core.UtilityModels;
using Microsoft.AspNetCore.Identity;
namespace Core.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public double? MuscleWeight { get; set; }
        public double? FatWeight { get; set; }
        public string? ImageUrl { get; set; }
        public string? ActivityLevelName { get; set; }
        public ActivityLevel? ActivityLevel { get; set; }
        public double? GoalWeight { get; set; }
        public double? RecommendedCalories { get; set; }
        public Client? Client { get; set; }
        public Coach? Coach { get; set; }
        public string? MainGoal { get; set; }
        public Goal? Goal { get; set; }
        public List<FavouriteFood>? FavouriteFoods  { get; set; }
        public List<FoodRating>? FoodRatings { get; set; }
        public List<WorkoutPlan>? WorkoutPlans { get; set; }
        public List<WorkoutHistory>? WorkoutHistories { get; set; }
        public List<DietPlan>? DietPlans { get; set; }
        public List<UserDisease>? UserDiseases { get; set; }
    }
}
