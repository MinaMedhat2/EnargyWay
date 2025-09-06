// File: Company.Client/Models/CartOrderModel.cs
using System.Collections.Generic;
using System.Linq;

namespace Company.Client.Models
{
    // This model is specifically for creating an order from the shopping cart.
    public class CartOrderModel
    {
        // A single object that holds all shipping-related information.
        public ShippingInfoModel ShippingInfo { get; set; } = new();

        // A list of all items included in the order.
        public List<OrderItemDto> OrderItems { get; set; } = new();

        // The payment method chosen by the user.
        public string PaymentMethod { get; set; } = "Credit Card";

        // A calculated property that automatically sums up the total cost.
        public decimal TotalAmount => OrderItems.Sum(item => item.Price * item.Quantity);
    }
}
