using Microsoft.EntityFrameworkCore;

namespace Tournament.Data.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
        }

        public DbSet<Domain.Models.Entities.TournamentDetails> TournamentDetails { get; set; } = default!;
        public DbSet<Domain.Models.Entities.Game> Game { get; set; } = default!;
    }
}
