using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Exceptions;

public class InvalidOrderStateException : Exception
{
    public int OrderId { get; }
    public OrderStatus CurrentState { get; }

    public InvalidOrderStateException(int orderId, OrderStatus currentState)
        : base($"Order {orderId} is in state {currentState} and cannot be cancelled.")
    {
        OrderId = orderId;
        CurrentState = currentState;
    }
}
