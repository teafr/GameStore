using GameStore.Application.Abstractions;
using GameStore.Application.Contracts;
using System.Threading.Channels;

namespace GameStore.Infrastructure.BackgroundTasks;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<BackgroundWorkItem> queue = Channel.CreateUnbounded<BackgroundWorkItem>();

    public async ValueTask Enqueue(BackgroundWorkItem workItem)
    {
        ValidateWorkItem(workItem);
        await queue.Writer.WriteAsync(workItem);
    }

    public async Task<BackgroundWorkItem> DequeueAsync(CancellationToken cancellationToken)
    {
        return await queue.Reader.ReadAsync(cancellationToken);
    }

    private static void ValidateWorkItem(BackgroundWorkItem workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
    }
}