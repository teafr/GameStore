using Asp.Versioning;
using GameStore.API.ApiModels.Responses;
using GameStore.API.Cache;
using GameStore.Application.Models;
using GameStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PlatformsController : BaseController
{
    private readonly IPlatformService platformService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformsController"/>.
    /// </summary>
    /// <param name="platformService">Platform service for business operations.</param>
    public PlatformsController(IPlatformService platformService)
    {
        this.platformService = platformService;
    }

    /// <summary>
    /// Gets the list of all platforms.
    /// </summary>
    /// <returns>List of platforms.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<PlatformDto>), StatusCodes.Status200OK)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<PlatformDto>>> GetAll()
    {
        var platforms = await platformService.GetAll();
        return Ok(platforms);
    }

    /// <summary>
    /// Gets platform details by its unique identifier (GUID).
    /// </summary>
    /// <param name="id">Platform ID (GUID).</param>
    /// <returns>The found platform or 404 if not found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlatformDto>> GetById(Guid id)
    {
        var platform = await platformService.GetByIdAsync(id);
        return Ok(platform);
    }

    /// <summary>
    /// Gets all games available on a specific platform.
    /// </summary>
    /// <param name="platformId">Platform ID (GUID).</param>
    /// <returns>A list of games associated with the specified platform.</returns>
    [HttpGet("{platformId}/games")]
    [ProducesResponseType(typeof(List<GameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<GameDto>>> GetGames(Guid platformId)
    {
        var games = await platformService.GetGamesAsync(platformId);
        return Ok(games);
    }

    /// <summary>
    /// Adds a new platform to the store.
    /// </summary>
    /// <param name="dto">Platform data transfer object.</param>
    /// <returns>Created platform response.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PlatformDto>> Add([FromBody] PlatformDto dto)
    {
        var newPlatform = await platformService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newPlatform.Id }, newPlatform);
    }

    /// <summary>
    /// Updates an existing platform.
    /// </summary>
    /// <param name="dto">Platform data transfer object with updated data.</param>
    /// <returns>No content response upon success.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] PlatformDto dto)
    {
        await platformService.UpdateAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a platform by its unique identifier (GUID).
    /// </summary>
    /// <param name="id">ID of the platform to delete.</param>
    /// <returns>No content response when successful.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await platformService.DeleteAsync(id);
        return NoContent();
    }
}
