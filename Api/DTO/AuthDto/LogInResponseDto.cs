namespace Api.DTO.AuthDto
{
    public class LoginResponseDto
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
        public string Token { get; set; }

    }
}
