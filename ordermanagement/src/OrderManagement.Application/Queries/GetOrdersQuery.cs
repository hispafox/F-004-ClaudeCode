using MediatR;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Queries;

public record GetOrdersQuery() : IRequest<IReadOnlyList<Order>>;
