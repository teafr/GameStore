using GameStore.Application.Contracts;
using GameStore.Application.Abstractions;
using System.Collections.Concurrent;

namespace GameStore.Infrastructure.BackgroundTasks;

public class InMemoryTaskTracker : ITaskTracker
{
    private readonly ConcurrentDictionary<Guid, BackgroundTaskInfo> tasks = new();

    public void Register(BackgroundTaskInfo info) => tasks[info.Id] = info;

    public void ReportSuccess(Guid taskId) => UpdateTask(taskId, info => info.Status = BackgroundTaskStatus.Succeeded);

    public void ReportFailure(Guid taskId) => UpdateTask(taskId, info => info.Status = BackgroundTaskStatus.Failed);

    public BackgroundTaskInfo? GetStatus(Guid taskId) => GetTaskInfoById(taskId);

    private void UpdateTask(Guid taskId, Action<BackgroundTaskInfo> update)
    {
        if (tasks.TryGetValue(taskId, out var info))
        {
            update(info);
        }
    }

    private BackgroundTaskInfo? GetTaskInfoById(Guid id)
    {
        tasks.TryGetValue(id, out var info);
        return info;
    }
}