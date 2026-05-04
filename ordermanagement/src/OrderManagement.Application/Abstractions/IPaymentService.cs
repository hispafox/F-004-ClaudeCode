namespace OrderManagement.Application.Abstractions;

public interface IPaymentService
{
    Task<bool> ChargeAsync(int customerId, decimal amount, CancellationToken ct);
}
