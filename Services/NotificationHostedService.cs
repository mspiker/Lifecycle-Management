namespace LifecycleManagement.Services;

/// <summary>
/// Background service placeholder for future email notification engine.
/// </summary>
public class NotificationHostedService : BackgroundService
{
    private readonly ILogger<NotificationHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationHostedService(ILogger<NotificationHostedService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification service started. Checking every hour.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessNotificationsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing notifications");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ProcessNotificationsAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        // TODO: Implement notification logic
        _logger.LogDebug("Notification check completed (not yet implemented)");
        await Task.CompletedTask;
    }
}
