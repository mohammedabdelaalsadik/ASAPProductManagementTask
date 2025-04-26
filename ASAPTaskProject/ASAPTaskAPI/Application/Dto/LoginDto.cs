using System.ComponentModel.DataAnnotations;

namespace ASAPTaskAPI.Application.Dto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        public string Password { get; set; } = string.Empty;
    }
}
