namespace Api.DTO.AuthDto
{
    public class ChangePasswordDto
    {
        public string Id { get; set; }

        public string ChangePasswordTokken { get; set; }
        public string NewPassword { get; set; }
    }
}
