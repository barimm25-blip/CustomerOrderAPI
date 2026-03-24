namespace CustomerOrderAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public string? Remark { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Customer? Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<OrderStatusLog> StatusLogs { get; set; } = new List<OrderStatusLog>();
    }
}
