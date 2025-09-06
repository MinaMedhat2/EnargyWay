// File: Company.Client/Models/Order.cs
namespace Company.Client.Models
{
    // This is a copy of the Order model from the API project.
    // It's needed for the client to understand the data structure.
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        // This is the list of items within the order.
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
