namespace Company.API.DTOs
{
    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public int TotalQuantitySold { get; set; } // استخدمنا int لأنه يتناسب مع Quantity في موديل Sale
    }
}
