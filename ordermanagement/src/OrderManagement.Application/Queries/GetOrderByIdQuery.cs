using MediatR;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Queries;

public record GetOrderByIdQuery(int OrderId) : IRequest<Order>;
