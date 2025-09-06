using System.ComponentModel.DataAnnotations;

namespace Company.API.Models
{
    public class DeliveryAssignment
    {
        [Key]
        public int AssignmentId { get; set; } // المفتاح الأساسي لجدول مهام التوصيل

        public int OriginalOrderId { get; set; } // رقم الطلب الأصلي للرجوع إليه

        // --- نفس بيانات الطلب التي يحتاجها المندوب ---
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // --- بيانات المنتجات داخل الطلب (مهم جدًا) ---
        // سنخزنها كنص JSON لتبسيط الأمر
        public string OrderItemsJson { get; set; } = string.Empty;

        // --- يمكن إضافة بيانات المندوب هنا لاحقًا ---
        // public int? DeliveryPersonId { get; set; }
        // public string? DeliveryPersonName { get; set; }
    }
}
