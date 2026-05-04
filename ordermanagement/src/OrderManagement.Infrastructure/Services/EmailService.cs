using Microsoft.Extensions.Logging;
using OrderManagement.Application.Abstractions;

namespace OrderManagement.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public Task SendAsync(string to, string subject, string body, CancellationToken ct)
    {
        _logger.LogInformation(
            "[mock email] to={To} subject={Subject} body={Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
