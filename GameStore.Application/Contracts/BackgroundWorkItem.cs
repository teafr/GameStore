namespace GameStore.Application.Contracts;

public readonly struct BackgroundWorkItem
{
    public Guid JobId { get; }
    public BackgroundTaskDelegate WorkItem { get; }

    public BackgroundWorkItem(Guid jobId, BackgroundTaskDelegate workItem) : this()
    {
        JobId = jobId;
        WorkItem = workItem;
    }
}