// File: Company.API/DTOs/CreateOrderDto.cs
using System.ComponentModel.DataAnnotations;

namespace Company.API.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        [Required]
        public string CustomerPhone { get; set; } = string.Empty;
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
