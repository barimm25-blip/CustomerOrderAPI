using CustomerOrderAPI.DTOs;

namespace CustomerOrderAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync(string? status);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
        Task<bool> CancelAsync(int id);
    }
}
