using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.API.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty; // Fix

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty; // Fix

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }

        public int? UserID { get; set; }
        public User? User { get; set; }
    }
}
