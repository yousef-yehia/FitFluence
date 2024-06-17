namespace Api.DTO
{
    public class ClientRegisterRequestDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public int MuscleWeight { get; set; }
        public int FatWeight { get; set; }
        public IFormFile Photo { get; set; }
    }
}
