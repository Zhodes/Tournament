using Microsoft.AspNetCore.Mvc;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Core.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(
    [FromQuery] bool includeGames = false,
    [FromQuery] string? sortBy = null,
    [FromQuery] string? order = "asc")
        {
            IEnumerable<Tournament.Core.Entities.TournamentDetails> tournaments;

            if (includeGames)
                tournaments = await _unitOfWork.TournamentRepository.GetAllIncludingGamesAsync();
            else
                tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();

            tournaments = sortBy?.ToLower() switch
            {
                "title" => order == "desc" ? tournaments.OrderByDescending(t => t.Title) : tournaments.OrderBy(t => t.Title),
                "startdate" => order == "desc" ? tournaments.OrderByDescending(t => t.StartDate) : tournaments.OrderBy(t => t.StartDate),
                _ => tournaments
            };

            var dtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null) return NotFound($"Game with ID {id} not found.");
            var dto = _mapper.Map<TournamentDto>(tournament);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentForUpdateDto dto)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null)
                return NotFound();

            tournament.Title = dto.Title;
            tournament.StartDate = dto.StartDate;

            tournament.Games.Clear();
            foreach (var gameId in dto.GameIds)
            {
                var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);
                if (game == null)
                    return BadRequest($"Game ID {gameId} not found.");

                tournament.Games.Add(game);
            }

            await _unitOfWork.CompleteAsync();
            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentForCreationDto tournamentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var tournamentEntity = _mapper.Map<TournamentDetails>(tournamentDto);

                foreach (var game in tournamentEntity.Games)
                {
                    game.TournamentDetails = tournamentEntity;
                }

                _unitOfWork.TournamentRepository.Add(tournamentEntity);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<TournamentDto>(tournamentEntity);
                return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournamentEntity.Id }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null) return NotFound();

            try
            {
                _unitOfWork.TournamentRepository.Remove(tournament);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TournamentDto>> PatchTournamentDetails(int id, JsonPatchDocument<TournamentDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null)
                return NotFound($"Tournament with ID {id} not found.");

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);

            patchDoc.ApplyTo(tournamentDto, error =>
            {
                ModelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage);
            });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(tournamentDto, tournament);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(tournamentDto);
        }

    }
}
