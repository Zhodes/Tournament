using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Game game);
        void Update(Game game);
        void Remove(Game game);
        Task<Game?> GetByTitleAsync(string title);
        Task<int> CountGamesByTournamentIdAsync(int tournamentId);

    }
}
