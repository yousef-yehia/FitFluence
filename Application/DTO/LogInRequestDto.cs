using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class LogInRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
