using CustomerOrderAPI.Data;
using CustomerOrderAPI.DTOs;
using CustomerOrderAPI.Models;
using CustomerOrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) { _context = context; }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products.Where(p => p.IsActive)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    Unit = p.Unit,
                    UnitPrice = p.UnitPrice,
                    StockQty = p.StockQty,
                    MinStockQty = p.MinStockQty
                }).ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null || !p.IsActive) return null;
            return new ProductDto
            {
                ProductId = p.ProductId,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                Unit = p.Unit,
                UnitPrice = p.UnitPrice,
                StockQty = p.StockQty,
                MinStockQty = p.MinStockQty
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                ProductCode = dto.ProductCode,
                ProductName = dto.ProductName,
                Unit = dto.Unit,
                UnitPrice = dto.UnitPrice,
                StockQty = dto.StockQty,
                MinStockQty = dto.MinStockQty
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(product.ProductId) ?? throw new Exception("Create failed");
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            product.ProductCode = dto.ProductCode;
            product.ProductName = dto.ProductName;
            product.Unit = dto.Unit;
            product.UnitPrice = dto.UnitPrice;
            product.StockQty = dto.StockQty;
            product.MinStockQty = dto.MinStockQty;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}