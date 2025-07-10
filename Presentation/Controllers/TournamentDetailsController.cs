using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournament.DTOs.Shared;
using Tournament.DTOs.Tournaments;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TournamentDetailsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TournamentDto>>> GetTournamentDetails(
    [FromQuery] bool includeGames = false,
    [FromQuery] string? sortBy = null,
    [FromQuery] string? order = "asc",
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
        {
            var result = await _service.TournamentService.GetAllAsync(includeGames, sortBy, order, page, pageSize);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var dto = await _service.TournamentService.GetByIdAsync(id);
            if (dto == null) return NotFound($"Tournament with ID {id} not found.");
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentForCreationDto dto)
        {
            var created = await _service.TournamentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTournamentDetails), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentForUpdateDto dto)
        {
            try
            {
                var success = await _service.TournamentService.UpdateAsync(id, dto);
                return success ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var success = await _service.TournamentService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TournamentDto>> PatchTournamentDetails(int id, JsonPatchDocument<TournamentDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            var patched = await _service.TournamentService.PatchAsync(id, patchDoc);
            return patched == null ? NotFound() : Ok(patched);
        }
    }
}
