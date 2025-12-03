using GameStore.Application.Contracts;
using GameStore.Application.Models;

namespace GameStore.Application.BackgroundTasks;

public interface IBackgroundJobService
{
    BackgroundTaskInfo GetStatus(Guid jobId);

    Task<Guid> StartGameFileGeneration(GameDto game);
}