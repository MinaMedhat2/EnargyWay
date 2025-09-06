// File: Company.Client/Models/OrderDto.cs
namespace Company.Client.Models
{
    // This class represents a single order record fetched from the database.
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; } // e.g., "Pending", "Shipped", "Delivered"
        public decimal TotalAmount { get; set; }

        // We will add the list of items later if needed.
        // For now, we just display the summary.
    }
}
