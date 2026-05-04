using Microsoft.Extensions.Logging;
using OrderManagement.Application.Abstractions;

namespace OrderManagement.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(ILogger<PaymentService> logger) => _logger = logger;

    public Task<bool> ChargeAsync(int customerId, decimal amount, CancellationToken ct)
    {
        _logger.LogInformation(
            "[mock payment] customerId={CustomerId} amount={Amount}", customerId, amount);
        return Task.FromResult(true);
    }
}
