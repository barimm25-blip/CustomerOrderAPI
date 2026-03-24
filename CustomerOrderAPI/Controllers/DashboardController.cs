using CustomerOrderAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context) { _context = context; }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var today = DateTime.Today;
            var result = new
            {
                TodayOrders = await _context.Orders.CountAsync(o => o.OrderDate.Date == today),
                TodayAmount = await _context.Orders.Where(o => o.OrderDate.Date == today).SumAsync(o => o.TotalAmount),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending"),
                ProcessingOrders = await _context.Orders.CountAsync(o => o.Status == "Processing"),
                CompletedOrders = await _context.Orders.CountAsync(o => o.Status == "Completed"),
                LowStockCount = await _context.Products.CountAsync(p => p.IsActive && p.StockQty <= p.MinStockQty)
            };
            return Ok(result);
        }

        [HttpGet("orders-by-status")]
        public async Task<IActionResult> GetOrdersByStatus()
        {
            var result = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count(), Total = g.Sum(o => o.TotalAmount) })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("monthly-report")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            var result = await _context.Orders
                .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count(), Total = g.Sum(o => o.TotalAmount) })
                .OrderBy(x => x.Date)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var result = await _context.Products
                .Where(p => p.IsActive && p.StockQty <= p.MinStockQty)
                .Select(p => new { p.ProductId, p.ProductCode, p.ProductName, p.StockQty, p.MinStockQty })
                .ToListAsync();
            return Ok(result);
        }
    }
}