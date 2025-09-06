using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.API.Models
{
    // Enum to define the type of action performed
    public enum ActionType
    {
        Add,
        Update,
        Delete
    }

    // Enum to define the target table of the action
    public enum TargetTable
    {
        Users,
        Products,
        Employees
    }

    public class AdminAction
    {
        [Key]
        public int ActionID { get; set; }

        [Required]
        public int AdminID { get; set; }

        [Required]
        public ActionType ActionType { get; set; }

        [Required]
        public TargetTable TargetTable { get; set; }

        [Required]
        public int TargetID { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        // Navigation property to link to the admin who performed the action
        [ForeignKey("AdminID")]
        public User? Admin { get; set; }
    }
}
