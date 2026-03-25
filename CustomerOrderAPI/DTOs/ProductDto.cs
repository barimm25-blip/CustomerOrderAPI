namespace CustomerOrderAPI.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQty { get; set; }
        public int MinStockQty { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class CreateProductDto
    {
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQty { get; set; }
        public int MinStockQty { get; set; }
        public string? ImageUrl { get; set; }
    }
}
