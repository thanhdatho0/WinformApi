using api.DTOs.Order;
using api.Models;

namespace api.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetAllAsync();
    public Task<Order?> GetByIdAsync(int id);
    public Task<Order> CreateAsync(Order order);

    public Task<List<Order>?> GetByCustomerIdAsync(int customerId);
    public Task<Order?> ConfirmOrder(int id);
}