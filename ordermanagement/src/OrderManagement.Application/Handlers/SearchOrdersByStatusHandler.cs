using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class SearchOrdersByStatusHandler
    : IRequestHandler<SearchOrdersByStatusQuery, IReadOnlyList<Order>>
{
    private readonly IOrderRepository _orders;

    public SearchOrdersByStatusHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public Task<IReadOnlyList<Order>> Handle(
        SearchOrdersByStatusQuery request,
        CancellationToken ct)
    {
        return _orders.GetByStatusAsync(request.Status, ct);
    }
}
