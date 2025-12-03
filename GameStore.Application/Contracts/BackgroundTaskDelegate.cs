namespace GameStore.Application.Contracts;

public delegate Task BackgroundTaskDelegate(CancellationToken token, IServiceProvider provider);