using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournament.DTOs.Games;
using Tournament.DTOs.Shared;
using Tournament.Services;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public GamesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GameDto>>> GetGame(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] string? sortBy = null,
    [FromQuery] string? order = "asc")
        {
            var pagedGames = await _service.GameService.GetAllGamesAsync(page, pageSize, sortBy, order);
            return Ok(pagedGames); // Metadata is included in response body
        }


        [HttpGet("{title}")]
        public async Task<ActionResult<GameDto>> GetGame(string title)
        {
            var game = await _service.GameService.GetGameByTitleAsync(title);
            if (game == null) return NotFound($"Game with Title {title} not found.");
            return Ok(game);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameForUpdateDto gameDto)
        {
            var result = await _service.GameService.UpdateGameAsync(id, gameDto);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameForCreationDto gameDto)
        {
            var createdGame = await _service.GameService.CreateGameAsync(gameDto);
            return CreatedAtAction(nameof(GetGame), new { title = createdGame.Title }, createdGame);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _service.GameService.DeleteGameAsync(id);
            return result;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GameDto>> PatchGame(int id, JsonPatchDocument<GameDto> patchDoc)
        {
            var result = await _service.GameService.PatchGameAsync(id, patchDoc, ModelState);
            return result;
        }
    }
}