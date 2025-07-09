using Microsoft.AspNetCore.JsonPatch;
using Tournament.DTOs.Tournaments;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDto>> GetAllAsync(bool includeGames, string? sortBy, string? order);
    Task<TournamentDto?> GetByIdAsync(int id);
    Task<TournamentDto> CreateAsync(TournamentForCreationDto dto);
    Task<bool> UpdateAsync(int id, TournamentForUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<TournamentDto?> PatchAsync(int id, JsonPatchDocument<TournamentDto> patchDoc);
}
