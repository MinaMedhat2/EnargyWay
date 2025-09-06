// File: Company.API/Models/OrderItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.API.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        // --- Foreign Keys ---
        [Required]
        public int OrderId { get; set; } // Link to the Order
        [Required]
        public int ProductId { get; set; } // Link to the Product

        // --- Item Details ---
        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } // Price of the product at the time of order

        // --- Navigation Properties ---
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
