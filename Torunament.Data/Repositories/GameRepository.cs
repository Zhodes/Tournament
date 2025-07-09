using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using Tournament.Data.Data;
using Domain.Contracts.Repositories;

namespace Tournament.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentApiContext _context;

        public GameRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public void Add(Game game)
        {
            _context.Game.Add(game);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Game.AnyAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Game
                .Include(g => g.TournamentDetails)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Game
                .Include(g => g.TournamentDetails)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game?> GetByTitleAsync(string title)
        {
            return await _context.Game
                .Include(g => g.TournamentDetails)
                .FirstOrDefaultAsync(g => g.Title == title);
        }

        public void Remove(Game game)
        {
            _context.Game.Remove(game);
        }

        public void Update(Game game)
        {
            _context.Game.Update(game);
        }
    }
}
