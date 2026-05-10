using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Commands;

namespace OrderManagement.Application.Handlers;

public class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Unit>
{
    private readonly IOrderRepository _orders;

    public RemoveItemHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct) ?? throw new OrderNotFoundException(request.OrderId);
        order.Items.RemoveAll(i => i.Id == request.OrderItemId);
        await _orders.UpdateAsync(order, ct);
        return Unit.Value;
    }
}
