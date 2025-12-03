using Asp.Versioning;
using GameStore.API.ApiModels.Responses;
using GameStore.API.Cache;
using GameStore.Application.Models;
using GameStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GenresController : BaseController
{
    private readonly IGenreService genreService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenresController"/>.
    /// </summary>
    /// <param name="genreService">Genre service for business operations.</param>
    public GenresController(IGenreService genreService)
    {
        this.genreService = genreService;
    }

    /// <summary>
    /// Gets the list of all genres.
    /// </summary>
    /// <returns>List of genres.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<GenreDto>), StatusCodes.Status200OK)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<GenreDto>>> GetAll()
    {
        var genres = await genreService.GetAll();
        return Ok(genres);
    }

    /// <summary>
    /// Gets the genre details by its unique identifier (GUID).
    /// </summary>
    /// <param name="id">Genre ID (GUID).</param>
    /// <returns>The found genre or 404 if not found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenreDto>> GetById(Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);
        return Ok(genre);
    }

    /// <summary>
    /// Gets all sub-genres of a specific parent genre.
    /// </summary>
    /// <param name="parentGenreId">Parent Genre ID (GUID).</param>
    /// <returns>List of sub-genres.</returns>
    [HttpGet("{parentGenreId}/subgenres")]
    [ProducesResponseType(typeof(List<GenreDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<GenreDto>>> GetSubGenres(Guid parentGenreId)
    {
        var subGenres = await genreService.GetByParentIdAsync(parentGenreId);
        return Ok(subGenres);
    }

    /// <summary>
    /// Gets all games belonging to a specific genre.
    /// </summary>
    /// <param name="genreId">Genre ID (GUID).</param>
    /// <returns>A list of games associated with the specified genre.</returns>
    [HttpGet("{genreId}/games")]
    [ProducesResponseType(typeof(List<GameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ResponseCache(CacheProfileName = CacheProfiles.Default)]
    public async Task<ActionResult<List<GameDto>>> GetGames(Guid genreId)
    {
        var games = await genreService.GetGamesAsync(genreId);
        return Ok(games);
    }

    /// <summary>
    /// Adds a new genre to the store.
    /// </summary>
    /// <param name="dto">Genre data transfer object.</param>
    /// <returns>No content response upon success.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Add([FromBody] GenreDto dto)
    {
        GenreDto newGenre = await genreService.Add(dto);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, newGenre);
    }

    /// <summary>
    /// Updates an existing genre.
    /// </summary>
    /// <param name="dto">Genre data transfer object with updated data.</param>
    /// <returns>No content response upon success.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update([FromBody] GenreDto dto)
    {
        await genreService.UpdateAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a genre by its unique identifier (GUID).
    /// </summary>
    /// <param name="id">ID of the genre to delete.</param>
    /// <returns>No content response when successful.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }
}
