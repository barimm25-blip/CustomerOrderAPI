using System.ComponentModel.DataAnnotations;

namespace CustomerOrderAPI.Models
{
    public class OrderStatusLog
    {
        [Key]
        public int LogId { get; set; }
        public int OrderId { get; set; }
        public string? OldStatus { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;

        public Order? Order { get; set; }
    }
}
