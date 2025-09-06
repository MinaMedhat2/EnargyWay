using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.API.Models
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        // --- THESE COLUMNS ARE MISSING IN YOUR DATABASE ---
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerUnit { get; set; } // This is the missing column

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        // ---------------------------------------------------

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProductID")]
        public Product? Product { get; set; }
    }
}
