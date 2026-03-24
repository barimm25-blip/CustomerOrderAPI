namespace CustomerOrderAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQty { get; set; }
        public int MinStockQty { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
