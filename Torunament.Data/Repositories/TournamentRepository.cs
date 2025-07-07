using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Torunament.Data.Repositories
{
    internal class TournamentRepository :  ITournamentRepository
    {

        private readonly TournamentApiContext _context;

        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public void Add(TournamentDetails tournament)
        {
            _context.TournamentDetails.Add(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.TournamentDetails.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
        {
            return await _context.TournamentDetails
                .ToListAsync();
        }

        public async Task<IEnumerable<TournamentDetails>> GetAllIncludingGamesAsync()
        {
            return await _context.TournamentDetails
                                 .Include(t => t.Games)
                                 .ToListAsync();
        }

        public async Task<TournamentDetails?> GetByIdAsync(int id)
        {
            return await _context.TournamentDetails
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Remove(TournamentDetails tournament)
        {
            _context.TournamentDetails.Remove(tournament);
        }

        public void Update(TournamentDetails tournament)
        {
            _context.TournamentDetails.Update(tournament);
        }
    }
}
