using System.Text.Json.Serialization;

namespace FoodOrderingAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();
    }
}
