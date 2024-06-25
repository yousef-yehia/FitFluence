namespace Api.DTO.CoachDto
{
    public class CoachReturnDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public double? MuscleWeight { get; set; }
        public double? FatWeight { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string CvUrl { get; set; }
    }
}
