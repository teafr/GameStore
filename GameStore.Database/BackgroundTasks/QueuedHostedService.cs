using GameStore.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameStore.Infrastructure.BackgroundTasks;

public class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue taskQueue;
    private readonly IServiceProvider provider;
    private readonly ILogger<QueuedHostedService> logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue, IServiceProvider provider, ILogger<QueuedHostedService> logger)
    {
        this.taskQueue = taskQueue;
        this.provider = provider;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Queued Hosted Service is running.");
        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await taskQueue.DequeueAsync(stoppingToken);
            try
            {
                using var scope = provider.CreateScope();
                await workItem.WorkItem(stoppingToken, scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in background task.");
            }
        }
    }
}
