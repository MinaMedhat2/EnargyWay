using System.ComponentModel.DataAnnotations;

namespace Company.API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty; // Fix for CS8618

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Fix for CS8618

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty; // Fix for CS8618
    }
}
