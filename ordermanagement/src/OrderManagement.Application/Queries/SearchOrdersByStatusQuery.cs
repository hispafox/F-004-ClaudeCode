using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Queries;

public record SearchOrdersByStatusQuery(OrderStatus Status) : IRequest<IReadOnlyList<Order>>;
