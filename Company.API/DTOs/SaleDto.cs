namespace Company.API.DTOs
{
    public class SaleDto
    {
        public int SaleID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
