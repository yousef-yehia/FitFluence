namespace Api.DTO.AuthDto
{
    public class UserReturnDto
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string? MainGoal { get; set; }
        public string? ActivityLevel { get; set; }
        public double? GoalWeight { get; set; }
        public double? MuscleWeight { get; set; }
        public double? FatWeight { get; set; }
        public double? RecommendedCalories { get; set; }
    }
}
