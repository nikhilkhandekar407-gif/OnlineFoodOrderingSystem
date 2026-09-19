using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/fooditems
        // GET: api/fooditems?categoryId=1&search=pizza
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems(
            [FromQuery] int? categoryId, [FromQuery] string? search)
        {
            var query = _context.FoodItems.Include(f => f.Category).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(f => f.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(f => f.Name.Contains(search));

            var items = await query
                .Select(f => new FoodItemDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    Price = f.Price,
                    ImageUrl = f.ImageUrl,
                    IsAvailable = f.IsAvailable,
                    IsVeg = f.IsVeg,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category != null ? f.Category.Name : null
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/fooditems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetFoodItem(int id)
        {
            var f = await _context.FoodItems.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (f == null) return NotFound(new { message = "Food item not found" });

            return Ok(new FoodItemDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Price = f.Price,
                ImageUrl = f.ImageUrl,
                IsAvailable = f.IsAvailable,
                IsVeg = f.IsVeg,
                CategoryId = f.CategoryId,
                CategoryName = f.Category?.Name
            });
        }

        // POST: api/fooditems
        [HttpPost]
        public async Task<ActionResult<FoodItemDto>> CreateFoodItem(CreateFoodItemDto dto)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists) return BadRequest(new { message = "Invalid categoryId" });

            var item = new FoodItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsAvailable = dto.IsAvailable,
                IsVeg = dto.IsVeg,
                CategoryId = dto.CategoryId
            };

            _context.FoodItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFoodItem), new { id = item.Id }, item);
        }

        // PUT: api/fooditems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodItem(int id, CreateFoodItemDto dto)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item == null) return NotFound(new { message = "Food item not found" });

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.ImageUrl = dto.ImageUrl;
            item.IsAvailable = dto.IsAvailable;
            item.IsVeg = dto.IsVeg;
            item.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/fooditems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item == null) return NotFound(new { message = "Food item not found" });

            _context.FoodItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
