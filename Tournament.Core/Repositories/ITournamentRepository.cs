using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<TournamentDetails>> GetAllAsync();
        Task<TournamentDetails?> GetByIdAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(TournamentDetails tournament);
        void Update(TournamentDetails tournament);
        void Remove(TournamentDetails tournament);
        Task<IEnumerable<TournamentDetails>> GetAllIncludingGamesAsync();
    }
}
