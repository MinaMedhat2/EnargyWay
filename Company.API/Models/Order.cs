// File: Company.API/Models/Order.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.API.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // --- Customer Information ---
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string CustomerEmail { get; set; } = string.Empty;
        [Required]
        public string CustomerPhone { get; set; } = string.Empty;
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        // --- Order Details ---
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        [Required]
        public string OrderStatus { get; set; } = "Pending"; // e.g., Pending, AssignedToDelivery, Completed

        // --- Delivery Information (as you requested) ---
        public string? DeliveryPersonName { get; set; } // Nullable, assigned later
        public string? DeliveryPersonPhone { get; set; } // Nullable, assigned later

        // --- Navigation Property ---
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
