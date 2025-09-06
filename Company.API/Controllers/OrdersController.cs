// File: Company.API/Controllers/OrdersController.cs
using Company.API.Data;
using Company.API.DTOs;
using Company.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Company.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders (يجلب كل الطلبات النشطة)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetActiveOrders()
        {
            return await _context.Orders
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToListAsync();
        }

        // GET: api/orders/assignments (يجلب مهام التوصيل)
        [HttpGet("assignments")]
        public async Task<ActionResult<IEnumerable<DeliveryAssignment>>> GetDeliveryAssignments()
        {
            return await _context.DeliveryAssignments
                                 .OrderByDescending(a => a.AssignedAt)
                                 .ToListAsync();
        }

        // POST: api/orders (إنشاء طلب جديد)
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            // ... (الكود الحالي لإنشاء الطلب يبقى كما هو)
            if (!ModelState.IsValid || !orderDto.OrderItems.Any()) return BadRequest("Invalid order data.");
            var productIds = orderDto.OrderItems.Select(oi => oi.ProductId).ToList();
            var productsInDb = await _context.Products.Where(p => productIds.Contains(p.ProductID)).ToDictionaryAsync(p => p.ProductID);
            if (productsInDb.Count != productIds.Count) return BadRequest("One or more products not found.");
            var newOrder = new Order { CustomerName = orderDto.CustomerName, CustomerEmail = orderDto.CustomerEmail, CustomerPhone = orderDto.CustomerPhone, ShippingAddress = orderDto.ShippingAddress, OrderStatus = "Pending", OrderDate = DateTime.UtcNow };
            decimal totalAmount = 0;
            foreach (var itemDto in orderDto.OrderItems)
            {
                var product = productsInDb[itemDto.ProductId];
                if (product.StockQuantity < itemDto.Quantity) return BadRequest($"Not enough stock for {product.Name}.");
                product.StockQuantity -= itemDto.Quantity;
                var orderItem = new OrderItem { ProductId = itemDto.ProductId, Quantity = itemDto.Quantity, UnitPrice = product.Price };
                newOrder.OrderItems.Add(orderItem);
                totalAmount += product.Price * itemDto.Quantity;
            }
            newOrder.TotalAmount = totalAmount;
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.OrderId }, newOrder);
        }

        // PUT: api/orders/{id}/status (تحديث حالة الطلب)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusDto statusDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == id);
                if (order == null) return NotFound($"Order with ID {id} not found.");

                string newStatus = statusDto.NewStatus;

                // تحديث حالة الطلب في الجدول الأصلي
                order.OrderStatus = newStatus;

                // إذا كانت الحالة هي "خارج للتوصيل"
                if (newStatus == "Out for Delivery")
                {
                    // 1. نتحقق إذا كانت هناك مهمة توصيل موجودة بالفعل لهذا الطلب لتجنب التكرار
                    var existingAssignment = await _context.DeliveryAssignments.FirstOrDefaultAsync(a => a.OriginalOrderId == id);
                    if (existingAssignment == null)
                    {
                        // 2. إنشاء مهمة توصيل جديدة (نسخ البيانات)
                        var assignment = new DeliveryAssignment
                        {
                            OriginalOrderId = order.OrderId,
                            CustomerName = order.CustomerName,
                            CustomerPhone = order.CustomerPhone,
                            ShippingAddress = order.ShippingAddress,
                            TotalAmount = order.TotalAmount,
                            AssignedAt = DateTime.UtcNow,
                            OrderItemsJson = JsonSerializer.Serialize(order.OrderItems.Select(oi => new {
                                oi.ProductId,
                                oi.Product.Name,
                                oi.Quantity,
                                oi.UnitPrice
                            }))
                        };
                        _context.DeliveryAssignments.Add(assignment);
                    }
                }

                // *** تم حذف السطر الذي كان يمسح الطلب من هنا ***

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = $"Order {id} status updated to {newStatus}." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An internal error occurred: {ex.Message}");
            }
        }

        // PUT: api/orders/assignments/{assignmentId}/deliver (تأكيد التسليم)
        [HttpPut("assignments/{assignmentId}/deliver")]
        public async Task<IActionResult> MarkAsDelivered(int assignmentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var assignment = await _context.DeliveryAssignments.FindAsync(assignmentId);
                if (assignment == null) return NotFound($"Delivery assignment with ID {assignmentId} not found.");

                // --- بداية التعديل الأهم ---
                // 1. البحث عن الطلب الأصلي لحذفه
                var originalOrder = await _context.Orders.FindAsync(assignment.OriginalOrderId);
                if (originalOrder != null)
                {
                    _context.Orders.Remove(originalOrder);
                }
                // --- نهاية التعديل الأهم ---

                // 2. تسجيل المبيعات
                var orderItems = JsonSerializer.Deserialize<List<JsonOrderItem>>(assignment.OrderItemsJson);
                if (orderItems != null)
                {
                    foreach (var item in orderItems)
                    {
                        _context.Sales.Add(new Sale { ProductID = item.ProductId, Quantity = item.Quantity, SaleDate = DateTime.UtcNow, TotalPrice = item.UnitPrice * item.Quantity });
                    }
                }

                // 3. أرشفة الطلب
                _context.CompletedOrders.Add(new CompletedOrder { OriginalOrderId = assignment.OriginalOrderId, CustomerName = assignment.CustomerName, CustomerPhone = assignment.CustomerPhone, ShippingAddress = assignment.ShippingAddress, OrderDate = assignment.AssignedAt, TotalAmount = assignment.TotalAmount, FinalStatus = "Delivered", CompletionDate = DateTime.UtcNow });

                // 4. حذف المهمة من جدول مهام التوصيل
                _context.DeliveryAssignments.Remove(assignment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = $"Assignment {assignmentId} has been marked as delivered and archived." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An internal error occurred: {ex.Message}");
            }
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return NotFound();
            return order;
        }

        private class JsonOrderItem { public int ProductId { get; set; } public string Name { get; set; } public int Quantity { get; set; } public decimal UnitPrice { get; set; } }
    }
}
