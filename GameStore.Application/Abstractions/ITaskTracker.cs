using GameStore.Application.Contracts;

namespace GameStore.Application.Abstractions;

public interface ITaskTracker
{
    BackgroundTaskInfo? GetStatus(Guid taskId);

    void Register(BackgroundTaskInfo info);

    void ReportFailure(Guid taskId);

    void ReportSuccess(Guid taskId);
}