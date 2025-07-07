using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournament.Core.Dtos;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame(
     [FromQuery] string? sortBy = null,
     [FromQuery] string? order = "asc")
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();

            games = sortBy?.ToLower() switch
            {
                "title" => order == "desc" ? games.OrderByDescending(g => g.Title) : games.OrderBy(g => g.Title),
                "time" => order == "desc" ? games.OrderByDescending(g => g.Time) : games.OrderBy(g => g.Time),
                _ => games
            };

            var dtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(dtos);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<GameDto>> GetGame(string title)
        {
            var game = await _unitOfWork.GameRepository.GetByTitleAsync(title);
            if (game == null) return NotFound($"Game with Title {title} not found.");

            var dto = _mapper.Map<GameDto>(game);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameForUpdateDto gameDto)
        {
            if (id != gameDto.Id)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingGame = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (existingGame == null)
                return NotFound($"Game with ID {id} not found.");

            _mapper.Map(gameDto, existingGame);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while updating game.");
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameForCreationDto gameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var gameEntity = _mapper.Map<Game>(gameDto);
                _unitOfWork.GameRepository.Add(gameEntity);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<GameDto>(gameEntity);
                return CreatedAtAction(nameof(GetGame), new { id = gameEntity.Id }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (game == null) return NotFound();

            try
            {
                _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GameDto>> PatchTournamentDetails(int id, JsonPatchDocument<GameDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (game == null)
                return NotFound($"Game with ID {id} not found.");

            var GameDto = _mapper.Map<GameDto>(game);

            patchDoc.ApplyTo(GameDto, error =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage);
            });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(GameDto, game);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(GameDto);
        }

    }
}
