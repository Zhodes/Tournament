using Microsoft.AspNetCore.JsonPatch;
using Tournament.DTOs.Games;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync(string? sortBy, string? order);
    Task<GameDto?> GetByTitleAsync(string title);
    Task<GameDto> CreateAsync(GameForCreationDto dto);
    Task<bool> UpdateAsync(int id, GameForUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<GameDto?> PatchAsync(int id, JsonPatchDocument<GameDto> patchDoc);
}
