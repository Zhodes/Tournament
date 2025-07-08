using Microsoft.EntityFrameworkCore;

namespace Domain.Presentation.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
        }

        public DbSet<Domain.Core.Entities.TournamentDetails> TournamentDetails { get; set; } = default!;
        public DbSet<Domain.Core.Entities.Game> Game { get; set; } = default!;
    }
}
