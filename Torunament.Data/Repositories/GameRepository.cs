using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

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

        //public async Task<IEnumerable<Game>> FindAsync(Expression<Func<Game, bool>> predicate)
        //{
        //    return await _context.Games.Where(predicate).ToListAsync();
        //}

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Game
                .Include(g => g.TournamentDetails) // optional: include Tournament if you want extra data
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Game
                .Include(g => g.TournamentDetails) // optional: include Tournament
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game?> GetByTitleAsync(string title)
        {
            return await _context.Game
                .Include(g => g.TournamentDetails) // optional: include Tournament
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
