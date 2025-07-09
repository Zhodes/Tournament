using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tournament.DTOs.Games;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllGamesAsync(string? sortBy, string? order);
    Task<ActionResult<GameDto>> GetGameByTitleAsync(string title);
    Task<GameDto> CreateGameAsync(GameForCreationDto dto);
    Task<IActionResult> UpdateGameAsync(int id, GameForUpdateDto dto);
    Task<IActionResult> DeleteGameAsync(int id);
    Task<ActionResult<GameDto>> PatchGameAsync(int id, JsonPatchDocument<GameDto> patchDoc, ModelStateDictionary modelState);
}
