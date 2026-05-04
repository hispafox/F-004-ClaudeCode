using MediatR;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Commands;

public record UpdateOrderCommand(int OrderId, OrderStatus NewStatus) : IRequest;
