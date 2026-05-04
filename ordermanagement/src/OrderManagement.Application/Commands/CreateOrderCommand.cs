using MediatR;

namespace OrderManagement.Application.Commands;

public record CreateOrderItemDto(string ProductName, int Quantity, decimal UnitPrice);

public record CreateOrderCommand(int CustomerId, List<CreateOrderItemDto> Items) : IRequest<int>;
