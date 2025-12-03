using GameStore.Application.Contracts;

namespace GameStore.Application.Abstractions;

public interface IBackgroundTaskQueue
{
    ValueTask Enqueue(BackgroundWorkItem workItem);

    Task<BackgroundWorkItem> DequeueAsync(CancellationToken cancellationToken);
}