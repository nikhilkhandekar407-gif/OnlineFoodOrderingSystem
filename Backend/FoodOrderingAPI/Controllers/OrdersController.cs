using System.Security.Claims;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCustomerId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idClaim!);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                return BadRequest(new { message = "Order must contain at least one item" });

            var customerId = GetCustomerId();

            var order = new Order
            {
                CustomerId = customerId,
                DeliveryAddress = dto.DeliveryAddress,
                ContactPhone = dto.ContactPhone,
                PaymentMethod = dto.PaymentMethod,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow
            };

            decimal total = 0;

            foreach (var line in dto.Items)
            {
                var food = await _context.FoodItems.FindAsync(line.FoodItemId);
                if (food == null || !food.IsAvailable)
                    return BadRequest(new { message = $"Food item {line.FoodItemId} is not available" });

                if (line.Quantity <= 0)
                    return BadRequest(new { message = "Quantity must be greater than zero" });

                var orderItem = new OrderItem
                {
                    FoodItemId = food.Id,
                    FoodItemName = food.Name,
                    Quantity = line.Quantity,
                    UnitPrice = food.Price
                };

                total += orderItem.LineTotal;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, MapToDto(order));
        }

        // GET: api/orders/my
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetMyOrders()
        {
            var customerId = GetCustomerId();

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders.Select(MapToDto));
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
        {
            var customerId = GetCustomerId();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == customerId);

            if (order == null) return NotFound(new { message = "Order not found" });

            return Ok(MapToDto(order));
        }

        // PUT: api/orders/5/status  (admin/staff use)
        [HttpPut("{id}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound(new { message = "Order not found" });

            order.Status = dto.Status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static OrderResponseDto MapToDto(Order o) => new OrderResponseDto
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            Status = o.Status.ToString(),
            DeliveryAddress = o.DeliveryAddress,
            ContactPhone = o.ContactPhone,
            PaymentMethod = o.PaymentMethod,
            TotalAmount = o.TotalAmount,
            Items = o.OrderItems.Select(oi => new OrderItemResponseDto
            {
                FoodItemId = oi.FoodItemId,
                FoodItemName = oi.FoodItemName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                LineTotal = oi.LineTotal
            }).ToList()
        };
    }
}
