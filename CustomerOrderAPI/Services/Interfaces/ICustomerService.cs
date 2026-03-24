using CustomerOrderAPI.DTOs;

namespace CustomerOrderAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync(string? search);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<bool> UpdateAsync(int id, CreateCustomerDto dto);
        Task<bool> DeleteAsync(int id);
    }
}