namespace Api.DTO.AuthDto
{
    public class CoachRegisterRequestDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public double MuscleWeight { get; set; }
        public double FatWeight { get; set; }
        public IFormFile Cv { get; set; }
    }
}
