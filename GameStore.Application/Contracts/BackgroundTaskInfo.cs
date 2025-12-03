namespace GameStore.Application.Contracts;

public class BackgroundTaskInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public BackgroundTaskStatus Status { get; set; } = BackgroundTaskStatus.Queued;
}
