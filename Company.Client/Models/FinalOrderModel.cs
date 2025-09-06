// File: Company.Client/Models/FinalOrderModel.cs
namespace Company.Client.Models
{
    // This class holds ALL the information needed to create the final order.
    public class FinalOrderModel
    {
        // From Shipping Page
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public int Quantity { get; set; }

        // From Payment Page
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
