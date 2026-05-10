using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Handlers;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orders;

    public CancelOrderHandler(IOrderRepository orders) => _orders = orders;

    public async Task Handle(CancelOrderCommand request, CancellationToken ct)
    {
        var order = _orders.GetByIdAsync(request.OrderId, ct).Result
            ?? throw new OrderNotFoundException(request.OrderId);

        if (order.Status is not (OrderStatus.Pending or OrderStatus.Confirmed))
        {
            throw new InvalidOrderStateException(order.Id, order.Status);
        }

        order.Status = OrderStatus.Cancelled;
        await _orders.UpdateAsync(order, ct);
    }
}
