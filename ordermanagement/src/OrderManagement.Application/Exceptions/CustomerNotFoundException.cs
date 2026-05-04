namespace OrderManagement.Application.Exceptions;

public class CustomerNotFoundException : Exception
{
    public int CustomerId { get; }

    public CustomerNotFoundException(int customerId)
        : base($"Customer {customerId} not found.")
    {
        CustomerId = customerId;
    }
}
