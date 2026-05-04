using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IReadOnlyList<Order>>
{
    private readonly IOrderRepository _orders;

    public GetOrdersHandler(IOrderRepository orders) => _orders = orders;

    public Task<IReadOnlyList<Order>> Handle(GetOrdersQuery request, CancellationToken ct)
        => _orders.GetAllAsync(ct);
}
