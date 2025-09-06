using System.ComponentModel.DataAnnotations;

namespace Company.API.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = string.Empty; // Fix for CS8618

        [Required]
        public string Password { get; set; } = string.Empty; // Fix for CS8618
    }
}
