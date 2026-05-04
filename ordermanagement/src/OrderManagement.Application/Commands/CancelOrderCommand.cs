using MediatR;

namespace OrderManagement.Application.Commands;

public record CancelOrderCommand(int OrderId) : IRequest;
