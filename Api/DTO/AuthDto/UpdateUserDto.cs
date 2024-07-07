namespace Api.DTO.AuthDto
{
    public class UpdateUserDto
    {
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public int? Age { get; set; }
        public double? MuscleWeight { get; set; }
        public double? FatWeight { get; set; }
        public string? ImageUrl { get; set; }
        public string? FullName { get; set; }
        public string? MainGoal { get; set; }
        public string? ActivityLevel { get; set; }
        public double? GoalWeight { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
