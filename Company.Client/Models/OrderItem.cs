// File: Company.Client/Models/OrderItem.cs
namespace Company.Client.Models
{
    // This is a copy of the OrderItem model from the API project.
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // This will hold the product details that come from the API.
        public Product? Product { get; set; }
    }
}
