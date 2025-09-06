// File: Company.API/Models/CompletedOrder.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.API.Models
{
    // This table is for archiving completed or canceled orders.
    public class CompletedOrder
    {
        [Key]
        public int CompletedOrderId { get; set; }
        public int OriginalOrderId { get; set; } // To keep track of the original order ID

        // --- Mirrored Customer Information ---
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;

        // --- Mirrored Order Details ---
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public string FinalStatus { get; set; } = string.Empty; // "Completed" or "Canceled"
        public DateTime CompletionDate { get; set; } = DateTime.UtcNow;

        // --- Mirrored Delivery Information ---
        public string? DeliveryPersonName { get; set; }
        public string? DeliveryPersonPhone { get; set; }
    }
}
