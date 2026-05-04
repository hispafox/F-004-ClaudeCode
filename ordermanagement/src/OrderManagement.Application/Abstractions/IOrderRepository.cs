using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Abstractions;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct);
    Task<int> AddAsync(Order order, CancellationToken ct);
    Task UpdateAsync(Order order, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
