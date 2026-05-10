using MediatR;

namespace OrderManagement.Application.Commands;

public record RemoveItemCommand(int OrderId, int OrderItemId) : IRequest<Unit>;
