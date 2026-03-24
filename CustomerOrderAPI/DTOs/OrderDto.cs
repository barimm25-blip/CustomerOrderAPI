namespace CustomerOrderAPI.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? Remark { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string? Remark { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public string NewStatus { get; set; } = string.Empty;
        public string? ChangedBy { get; set; }
    }
}
