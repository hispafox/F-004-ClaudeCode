namespace OrderManagement.Application.Exceptions;

public class OrderNotFoundException : Exception
{
    public int OrderId { get; }

    public OrderNotFoundException(int orderId)
        : base($"Order {orderId} not found.")
    {
        OrderId = orderId;
    }
}
