namespace Company.Client.Models // <-- هذا هو التغيير الوحيد والحاسم
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }
        public int MaxAvailableStock { get; set; }
    }
}
