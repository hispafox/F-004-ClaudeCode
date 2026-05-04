using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly IOrderRepository _orders;

    public GetOrderByIdHandler(IOrderRepository orders) => _orders = orders;

    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        return await _orders.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);
    }
}
