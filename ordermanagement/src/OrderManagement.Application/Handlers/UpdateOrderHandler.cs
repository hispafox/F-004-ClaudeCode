using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;

namespace OrderManagement.Application.Handlers;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orders;

    public UpdateOrderHandler(IOrderRepository orders) => _orders = orders;

    public async Task Handle(UpdateOrderCommand request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct)
            ?? throw new OrderNotFoundException(request.OrderId);

        order.Status = request.NewStatus;
        await _orders.UpdateAsync(order, ct);
    }
}
