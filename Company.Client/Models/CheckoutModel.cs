// File: Company.Client/Models/CheckoutModel.cs
using System.ComponentModel.DataAnnotations;

namespace Company.Client.Models
{
    // This is the FINAL model that holds ALL order information.
    public class CheckoutModel
    {
        // --- From Shipping Page ---
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;

        // --- From Payment Page (Now set automatically) ---
        // We set the value directly here. No need for [Required] anymore.
        public string PaymentMethod { get; set; } = "CashOnDelivery";
    }
}
