using Company.API.Data;
using Company.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Company.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleCreateDto saleDto)
        {
            if (saleDto == null || saleDto.Quantity <= 0)
            {
                return BadRequest("Invalid sale data: Quantity must be greater than zero.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(saleDto.ProductId);
                if (product == null) return NotFound("Product not found.");
                if (product.StockQuantity < saleDto.Quantity) return BadRequest("Not enough stock.");

                product.StockQuantity -= saleDto.Quantity;

                // تحويل الوقت لتوقيت مصر
                var timeUtc = DateTime.UtcNow;
                var egyptZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                var egyptTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, egyptZone);

                product.UpdatedAt = egyptTime;
                _context.Entry(product).State = EntityState.Modified;

                var sale = new Sale
                {
                    ProductID = saleDto.ProductId,
                    Quantity = saleDto.Quantity,
                    PricePerUnit = product.Price,
                    TotalPrice = product.Price * saleDto.Quantity,
                    SaleDate = egyptTime
                };
                _context.Sales.Add(sale);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Sale recorded successfully and stock updated." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An internal error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // GET: api/Sales/today
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<SaleResponseDto>>> GetTodaysSales()
        {
            var egyptZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var nowInEgypt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);

            var startOfDay = nowInEgypt.Date;        // بداية اليوم (00:00)
            var endOfDay = startOfDay.AddDays(1);    // بداية اليوم التالي

            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startOfDay && s.SaleDate < endOfDay)
                .Include(s => s.Product)
                .Select(s => new SaleResponseDto
                {
                    SaleID = s.SaleID,
                    ProductName = s.Product.Name,
                    Quantity = s.Quantity,
                    TotalPrice = s.TotalPrice,
                    SaleDate = s.SaleDate
                })
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            return Ok(sales);
        }

        // GET: api/Sales/monthly
        [HttpGet("monthly")]
        public async Task<ActionResult<IEnumerable<SaleResponseDto>>> GetMonthlySales([FromQuery] int year, [FromQuery] int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startOfMonth && s.SaleDate < startOfNextMonth)
                .Include(s => s.Product)
                .Select(s => new SaleResponseDto
                {
                    SaleID = s.SaleID,
                    ProductName = s.Product.Name,
                    Quantity = s.Quantity,
                    TotalPrice = s.TotalPrice,
                    SaleDate = s.SaleDate
                })
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            return Ok(sales);
        }
    }

    // DTOs
    public class SaleCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class SaleResponseDto
    {
        public int SaleID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
