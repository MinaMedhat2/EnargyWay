using System.ComponentModel.DataAnnotations;

namespace Company.API.Models
{
    // --- بداية التصحيح النهائي ---
    // تعريف الأدوار بشكل صحيح ومنطقي.
    // هذا هو المصدر الوحيد للحقيقة الآن.
    public enum Role
    {
        User = 0,         // المستخدم العادي
        Admin = 1,        // الأدمن
        Employee = 2,     // مندوب التوصيل
        StoreManager = 3  // مدير المخزن
    }
    // --- نهاية التصحيح النهائي ---

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        // القيمة الافتراضية عند إنشاء مستخدم جديد ستكون "User"
        public Role Role { get; set; } = Role.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
