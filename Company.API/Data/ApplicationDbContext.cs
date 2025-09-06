using Company.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Company.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // --- كل الجداول معرفة هنا ---
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AdminAction> AdminActions { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }

        // الجدول الجديد الذي أضفناه
        public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Seed Admin User ---
            var simpleHash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes("admin"))
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Username = "admin",
                    Email = "admin@company.com",
                    PasswordHash = simpleHash,
                    Role = Role.Admin,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // --- العلاقات بين الجداول ---

            // 1. علاقة بين Order و OrderItem (واحد إلى متعدد)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // حذف بنود الطلب عند حذف الطلب

            // 2. علاقة بين Product و OrderItem (واحد إلى متعدد)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف المنتج إذا كان في أي طلب

            // --- بداية الإضافة الجديدة ---
            // 3. علاقة بين Employee و DeliveryAssignment (واحد إلى متعدد) - اختيارية
            // هذا يسمح لنا بتعيين مهمة توصيل لمندوب معين
            modelBuilder.Entity<DeliveryAssignment>()
                .HasOne<Employee>() // مهمة التوصيل لها مندوب واحد (اختياري)
                .WithMany() // المندوب يمكن أن يكون لديه مهام توصيل كثيرة
                .HasForeignKey("DeliveryPersonId") // اسم العمود الذي سيربط بينهما
                .IsRequired(false) // نجعل العلاقة اختيارية (يمكن أن تكون المهمة غير معينة)
                .OnDelete(DeleteBehavior.SetNull); // إذا تم حذف المندوب، تصبح المهمة غير معينة
            // --- نهاية الإضافة الجديدة ---
        }
    }
}
