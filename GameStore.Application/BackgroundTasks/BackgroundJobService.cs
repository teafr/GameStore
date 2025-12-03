using GameStore.Application.Abstractions;
using GameStore.Application.Contracts;
using GameStore.Application.Models;
using GameStore.Application.Services;
using GameStore.Domain.Exceptions.DatabaseExceptions;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Application.BackgroundTasks;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly IBackgroundTaskQueue taskQueue;
    private readonly ITaskTracker taskTracker;

    public BackgroundJobService(IBackgroundTaskQueue taskQueue, ITaskTracker taskTracker)
    {
        this.taskQueue = taskQueue;
        this.taskTracker = taskTracker;
    }

    public async Task<Guid> StartGameFileGeneration(GameDto game)
    {
        var taskInfo = new BackgroundTaskInfo();
        taskTracker.Register(taskInfo);

        await HandleJobEnqueue(taskInfo.Id, (token, provider) =>
        {
            var manager = provider.GetRequiredService<IGameFileManager>();
            return manager.GenerateFileAsync(game, token);
        });

        return taskInfo.Id;
    }

    public BackgroundTaskInfo GetStatus(Guid jobId)
    {
        return taskTracker.GetStatus(jobId) ?? throw new ObjectNotFoundException("Job", jobId);
    }

    private async Task HandleJobEnqueue(Guid id, BackgroundTaskDelegate workItem)
    {
        await taskQueue.Enqueue(new BackgroundWorkItem(id, async (token, provider) =>
        {
            try
            {
                await workItem(token, provider);
                taskTracker.ReportSuccess(id);
            }
            catch (Exception)
            {
                taskTracker.ReportFailure(id);
            }
        }));
    }
}