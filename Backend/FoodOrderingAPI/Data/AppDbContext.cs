using FoodOrderingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<FoodItem> FoodItems => Set<FoodItem>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItem>().Property(f => f.Price).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<FoodItem>()
                .HasOne(f => f.Category)
                .WithMany(c => c.FoodItems)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.FoodItem)
                .WithMany()
                .HasForeignKey(oi => oi.FoodItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Pizza", Description = "Wood-fired and classic pizzas", ImageUrl = "images/category-pizza.jpg" },
                new Category { Id = 2, Name = "Burgers", Description = "Juicy grilled and veg burgers", ImageUrl = "images/category-burger.jpg" },
                new Category { Id = 3, Name = "Indian", Description = "Curries, biryani and more", ImageUrl = "images/category-indian.jpg" },
                new Category { Id = 4, Name = "Desserts", Description = "Sweet endings to your meal", ImageUrl = "images/category-dessert.jpg" },
                new Category { Id = 5, Name = "Beverages", Description = "Cold drinks and shakes", ImageUrl = "images/category-beverage.jpg" }
            );

            modelBuilder.Entity<FoodItem>().HasData(
                new FoodItem { Id = 1, Name = "Margherita Pizza", Description = "Classic cheese and tomato pizza", Price = 249.00m, ImageUrl = "https://loremflickr.com/400/300/margherita,pizza,cheese", IsVeg = true, CategoryId = 1 },
                new FoodItem { Id = 2, Name = "Farmhouse Pizza", Description = "Loaded with garden veggies", Price = 349.00m, ImageUrl = "https://loremflickr.com/400/300/pizza,vegetables", IsVeg = true, CategoryId = 1 },
                new FoodItem { Id = 3, Name = "Chicken Pepperoni Pizza", Description = "Spicy pepperoni with mozzarella", Price = 399.00m, ImageUrl = "https://loremflickr.com/400/300/pepperoni,pizza,cheese", IsVeg = false, CategoryId = 1 },
                new FoodItem { Id = 4, Name = "Classic Veg Burger", Description = "Crispy veg patty with fresh veggies", Price = 129.00m, ImageUrl = "https://loremflickr.com/400/300/burger,vegetarian", IsVeg = true, CategoryId = 2 },
                new FoodItem { Id = 5, Name = "Chicken Zinger Burger", Description = "Crunchy fried chicken burger", Price = 179.00m, ImageUrl = "https://loremflickr.com/400/300/friedchicken,burger", IsVeg = false, CategoryId = 2 },
                new FoodItem { Id = 6, Name = "Paneer Butter Masala", Description = "Rich and creamy paneer curry", Price = 259.00m, ImageUrl = "https://loremflickr.com/400/300/paneer,indiancurry", IsVeg = true, CategoryId = 3 },
                new FoodItem { Id = 7, Name = "Chicken Biryani", Description = "Fragrant basmati rice with chicken", Price = 289.00m, ImageUrl = "https://loremflickr.com/400/300/biryani,rice", IsVeg = false, CategoryId = 3 },
                new FoodItem { Id = 8, Name = "Dal Makhani", Description = "Slow-cooked black lentils", Price = 219.00m, ImageUrl = "https://loremflickr.com/400/300/lentils,indiancurry", IsVeg = true, CategoryId = 3 },
                new FoodItem { Id = 9, Name = "Chocolate Brownie", Description = "Warm brownie with chocolate sauce", Price = 149.00m, ImageUrl = "https://loremflickr.com/400/300/chocolatebrownie,dessert", IsVeg = true, CategoryId = 4 },
                new FoodItem { Id = 10, Name = "Gulab Jamun (2 pcs)", Description = "Soft milk dumplings in sugar syrup", Price = 99.00m, ImageUrl = "https://loremflickr.com/400/300/indiansweet,dessert", IsVeg = true, CategoryId = 4 },
                new FoodItem { Id = 11, Name = "Cold Coffee", Description = "Chilled coffee with ice cream", Price = 119.00m, ImageUrl = "https://loremflickr.com/400/300/coldcoffee,milkshake", IsVeg = true, CategoryId = 5 },
                new FoodItem { Id = 12, Name = "Masala Lemonade", Description = "Refreshing spiced lemon drink", Price = 79.00m, ImageUrl = "https://loremflickr.com/400/300/lemonade,drink", IsVeg = true, CategoryId = 5 }
            );
        }
    }
}