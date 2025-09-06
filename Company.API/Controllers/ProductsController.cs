using Company.API.Data;
using Company.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Company.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.OrderByDescending(p => p.ProductID).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        // --- FIX for Low Stock Report ---
        // GET: api/Products/lowstock?threshold=15
        [HttpGet("lowstock")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            var lowStockProducts = await _context.Products
                .Where(p => p.StockQuantity > 0 && p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .Select(p => new ProductDto
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    StockQuantity = p.StockQuantity
                })
                .ToListAsync();

            return Ok(lowStockProducts);
        }

        // --- FIX for Top Selling Products Report ---
        // GET: api/products/topselling
        [HttpGet("topselling")]
        public async Task<ActionResult<IEnumerable<TopSellingProductDto>>> GetTopSellingProducts()
        {
            try
            {
                var topProducts = await _context.Sales
                    .GroupBy(sale => sale.ProductID)
                    .Select(group => new
                    {
                        ProductId = group.Key,
                        TotalQuantitySold = group.Sum(s => s.Quantity)
                    })
                    .OrderByDescending(p => p.TotalQuantitySold)
                    .Take(10)
                    .Join(
                        _context.Products,
                        saleInfo => saleInfo.ProductId,
                        product => product.ProductID,
                        (saleInfo, product) => new TopSellingProductDto
                        {
                            ProductId = product.ProductID,
                            Name = product.Name,
                            ImagePath = product.ImagePath,
                            TotalQuantitySold = saleInfo.TotalQuantitySold
                        })
                    .ToListAsync(); // Use ToListAsync for async operation

                return Ok(topProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // POST: api/Products (Handles file upload)
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductCreateDto productDto)
        {
            if (productDto == null || productDto.Image == null || productDto.Image.Length == 0)
            {
                return BadRequest("Product data or image file is missing.");
            }

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.Image.FileName);
            var imagePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await productDto.Image.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                ImagePath = $"/images/products/{uniqueFileName}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID) return BadRequest();

            var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductID == id);
            if (existingProduct != null)
            {
                product.ImagePath = existingProduct.ImagePath;
            }

            product.UpdatedAt = DateTime.UtcNow;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductID == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // --- DTOs (Data Transfer Objects) ---

    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }

    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int StockQuantity { get; set; }
    }

    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
