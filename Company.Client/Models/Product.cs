// --- File: Models/Product.cs ---

using System.Text.Json.Serialization;

namespace Company.Client.Models // تأكد من أن اسم المشروع صحيح
{
    public class Product
    {
        // نستخدم JsonPropertyName لمطابقة الأسماء مع الـ API تماماً
        [JsonPropertyName("productID")]
        public int ProductID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("imagePath")]
        public string? ImagePath { get; set; }

        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }
    }
}
