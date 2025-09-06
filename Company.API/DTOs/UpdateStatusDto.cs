// File: Company.API/DTOs/UpdateStatusDto.cs
using System.ComponentModel.DataAnnotations;

namespace Company.API.DTOs
{
    public class UpdateStatusDto
    {
        [Required]
        public string NewStatus { get; set; } = string.Empty;

        // Optional: You can add fields to update delivery info at the same time
        // For example:
        // public string? DeliveryPersonName { get; set; }
        // public string? DeliveryPersonPhone { get; set; }
    }
}
