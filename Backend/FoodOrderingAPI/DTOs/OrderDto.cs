using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.DTOs
{
    public class CreateOrderItemDto
    {
        public int FoodItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        public string DeliveryAddress { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Cash on Delivery";
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
