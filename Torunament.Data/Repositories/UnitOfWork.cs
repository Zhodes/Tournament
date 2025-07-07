using Torunament.Data.Repositories;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentApiContext _context;

        public IGameRepository GameRepository { get; }
        public ITournamentRepository TournamentRepository { get;}

        public UnitOfWork(TournamentApiContext context)
        {
            _context = context;
            TournamentRepository = new TournamentRepository(_context);
            GameRepository = new GameRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
