using CustomerOrderAPI.Data;
using CustomerOrderAPI.DTOs;
using CustomerOrderAPI.Models;
using CustomerOrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context) { _context = context; }

        public async Task<List<OrderDto>> GetAllAsync(string? status)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems).ThenInclude(i => i.Product)
                .AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);
            return await query.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderNo = o.OrderNo,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer!.CustomerName,
                OrderDate = o.OrderDate,
                RequestedDate = o.RequestedDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Remark = o.Remark,
                OrderItems = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product!.ProductName,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Amount = i.Amount
                }).ToList()
            }).ToListAsync();
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var o = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (o == null) return null;
            return new OrderDto
            {
                OrderId = o.OrderId,
                OrderNo = o.OrderNo,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer!.CustomerName,
                OrderDate = o.OrderDate,
                RequestedDate = o.RequestedDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Remark = o.Remark,
                OrderItems = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product!.ProductName,
                    Qty = i.Qty,
                    UnitPrice = i.UnitPrice,
                    Amount = i.Amount
                }).ToList()
            };
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {           
            var orderNo = $"ORD{DateTime.Now:yyyyMMddHHmmss}";
            var order = new Order
            {
                OrderNo = orderNo,
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.Now,
                RequestedDate = dto.RequestedDate,
                Status = "Pending",
                Remark = dto.Remark
            };

            decimal total = 0;
            foreach (var item in dto.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null) throw new Exception($"Product {item.ProductId} not found");
                if (product.StockQty < item.Qty) throw new Exception($"Stock not enough for {product.ProductName}");

                var amount = product.UnitPrice * item.Qty;
                total += amount;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    UnitPrice = product.UnitPrice,
                    Amount = amount
                });

                
                product.StockQty -= item.Qty;
            }

            order.TotalAmount = total;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(order.OrderId) ?? throw new Exception("Create failed");
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            // Business logic:  
            var flow = new[] { "Pending", "Confirmed", "Processing", "Shipped", "Completed" };
            var currentIndex = Array.IndexOf(flow, order.Status);
            var newIndex = Array.IndexOf(flow, dto.NewStatus);
            if (newIndex <= currentIndex) throw new Exception("Cannot move to previous status");

            // log
            _context.OrderStatusLogs.Add(new OrderStatusLog
            {
                OrderId = id,
                OldStatus = order.Status,
                NewStatus = dto.NewStatus,
                ChangedBy = dto.ChangedBy,
                ChangedAt = DateTime.Now
            });

            order.Status = dto.NewStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return false;
            if (order.Status != "Pending" && order.Status != "Confirmed")
                throw new Exception("Can only cancel Pending or Confirmed orders");

            
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null) product.StockQty += item.Qty;
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}