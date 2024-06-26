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
        public string? Name { get; set; }
    }
}
