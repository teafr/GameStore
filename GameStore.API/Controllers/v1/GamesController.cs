using Asp.Versioning;
using GameStore.API.ApiModels.Responses;
using GameStore.API.Cache;
using GameStore.Application.BackgroundTasks;
using GameStore.Application.Models;
using GameStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GamesController : BaseController
{
    private readonly IGameService gameService;
    private readonly IBackgroundJobService jobService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamesController"/>.
    /// </summary>
    /// <param name="gameService">Game service for business operations.</param>
    /// <param name="jobService">Service for orchestrating and tracking background jobs.</param>
    public GamesController(IGameService gameService, IBackgroundJobService jobService)
    {
        this.gameService = gameService;
        this.jobService = jobService;
    }

    /// <summary>
    /// Gets the list of all games.
    /// </summary>
    /// <returns>List of games.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<GameDto>), StatusCodes.Status200OK)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<GameDto>>> GetAll()
    {
        var games = await gameService.GetAllAsync();
        return Ok(games);
    }

    /// <summary>
    /// Gets the game details by its unique key.
    /// </summary>
    /// <param name="key">Game key.</param>
    /// <returns>The found game or 404 if not found.</returns>
    [HttpGet("key/{key}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<GameDto>> GetByKey(string key)
    {
        var game = await gameService.GetByKeyAsync(key);
        return Ok(game);
    }

    /// <summary>
    /// Gets the game details by its unique identifier (GUID).
    /// </summary>
    /// <param name="id">Game ID (GUID).</param>
    /// <returns>The found game or 404 if not found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<GameDto>> GetById(Guid id)
    {
        var game = await gameService.GetByIdAsync(id);
        return Ok(game);
    }

    /// <summary>
    /// Gets all genres for a given game.
    /// </summary>
    /// <param name="key">Game key.</param>
    /// <returns>List of genres.</returns>
    [HttpGet("{key}/genres")]
    [ProducesResponseType(typeof(List<GenreDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<GenreDto>>> GetGenres(string key)
    {
        var game = await gameService.GetByKeyAsync(key);
        if (game is null) return NotFound();

        var genres = await gameService.GetGenresAsync(key);
        return Ok(genres);
    }

    /// <summary>
    /// Gets all platforms for a given game.
    /// </summary>
    /// <param name="key">Game key.</param>
    /// <returns>List of platforms.</returns>
    [HttpGet("{key}/platforms")]
    [ProducesResponseType(typeof(List<PlatformDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<PlatformDto>>> GetPlatforms(string key)
    {
        var game = await gameService.GetByKeyAsync(key);
        if (game is null) return NotFound();

        var platforms = await gameService.GetPlatformsAsync(key);
        return Ok(platforms);
    }

    /// <summary>
    /// Adds a new game to the store.
    /// </summary>
    /// <param name="dto">Game data transfer object.</param>
    /// <returns>Created game response.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Add([FromBody] GameDto dto)
    {
        GameDto newGame = await gameService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, newGame);
    }

    /// <summary>
    /// Updates an existing game.
    /// </summary>
    /// <param name="dto">Game data transfer object with updated data.</param>
    /// <returns>No content response upon success.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] GameDto dto)
    {
        await gameService.UpdateAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a game by its unique key.
    /// </summary>
    /// <param name="key">Key of the game to delete.</param>
    /// <returns>No content response when successful.</returns>
    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteByKey(string key)
    {
        await gameService.DeleteByKeyAsync(key);
        return NoContent();
    }

    /// <summary>
    /// Starts asynchronous generation of the .txt file for a game by key.
    /// </summary>
    /// <param name="key">The unique game key.</param>
    /// <returns>
    /// HTTP 202 Accepted if the file generation job is started.  
    /// Returns a message indicating the task has been queued for processing.
    /// </returns>
    [HttpPost("{key}/file")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartGeneratingGameFile(string key)
    {
        var game = await gameService.GetByKeyAsync(key);
        var jobId = await jobService.StartGameFileGeneration(game);
        return Accepted(new { jobId, message = "File generation started." });
    }
}
