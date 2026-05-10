using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _orders;
    private readonly ICustomerRepository _customers;
    private readonly IEmailService _email;

    public CreateOrderHandler(
        IOrderRepository orders,
        ICustomerRepository customers,
        IEmailService email)
    {
        _orders = orders;
        _customers = customers;
        _email = email;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(request.CustomerId, ct)
            ?? throw new CustomerNotFoundException(request.CustomerId);

        var order = new Order
        {
            CustomerId = customer.Id,
            Items = request.Items
                .Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };

        var id = await _orders.AddAsync(order, ct);

        await _email.SendAsync(
            customer.Email,
            "Order received",
            $"Your order {id} has been received.",
            ct);

        return id;
    }
}
