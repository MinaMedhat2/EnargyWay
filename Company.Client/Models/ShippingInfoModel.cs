// File: Company.Client/Models/ShippingInfoModel.cs
using System.ComponentModel.DataAnnotations;

namespace Company.Client.Models
{
    // This model ONLY contains the information from the first page.
    public class ShippingInfoModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your shipping address.")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;

        public int MaxAvailableStock { get; set; }
    }
}
