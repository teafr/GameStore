using Asp.Versioning;
using GameStore.API.ApiModels.Responses;
using GameStore.Application.BackgroundTasks;
using GameStore.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tasks")]
public class BackgroundTasksController : BaseController
{
    private readonly IBackgroundJobService jobService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundTasksController"/>.
    /// </summary>
    /// <param name="jobService">Service for orchestrating and tracking background jobs.</param>
    public BackgroundTasksController(IBackgroundJobService jobService)
    {
        this.jobService = jobService;
    }

    /// <summary>
    /// Gets the status of a background job by its unique identifier.
    /// </summary>
    /// <param name="jobId">The unique identifier of the background job.</param>
    /// <returns>
    /// <see cref="BackgroundTaskInfo"/> containing information about the job's status.
    /// </returns>
    [HttpGet("{jobId}")]
    [ProducesResponseType(typeof(BackgroundTaskInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    public ActionResult<BackgroundTaskInfo> GetTaskStatus(Guid jobId)
    {
        var info = jobService.GetStatus(jobId);
        return Ok(info);
    }
}
