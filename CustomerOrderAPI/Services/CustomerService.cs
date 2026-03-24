using CustomerOrderAPI.Data;
using CustomerOrderAPI.DTOs;
using CustomerOrderAPI.Models;
using CustomerOrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;
        public CustomerService(AppDbContext context) { _context = context; }

        public async Task<List<CustomerDto>> GetAllAsync(string? search)
        {
            var query = _context.Customers.Where(c => c.IsActive);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.CustomerName.Contains(search) || c.CustomerCode.Contains(search));
            return await query.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerCode = c.CustomerCode,
                CustomerName = c.CustomerName,
                ContactName = c.ContactName,
                Phone = c.Phone,
                Email = c.Email
            }).ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var c = await _context.Customers.FindAsync(id);
            if (c == null || !c.IsActive) return null;
            return new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerCode = c.CustomerCode,
                CustomerName = c.CustomerName,
                ContactName = c.ContactName,
                Phone = c.Phone,
                Email = c.Email
            };
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                CustomerCode = dto.CustomerCode,
                CustomerName = dto.CustomerName,
                ContactName = dto.ContactName,
                Phone = dto.Phone,
                Email = dto.Email
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(customer.CustomerId) ?? throw new Exception("Create failed");
        }

        public async Task<bool> UpdateAsync(int id, CreateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;
            customer.CustomerCode = dto.CustomerCode;
            customer.CustomerName = dto.CustomerName;
            customer.ContactName = dto.ContactName;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;
            customer.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}