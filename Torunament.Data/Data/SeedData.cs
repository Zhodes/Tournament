using Bogus;
using Domain.Core.Entities;
using Domain.Presentation.Data;
using Microsoft.EntityFrameworkCore;

namespace Torunament.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(TournamentApiContext db)
        {
            await db.Database.MigrateAsync();

            if (await db.TournamentDetails.AnyAsync())
            {
                return; // Already seeded
            }

            try
            {
                var tournaments = GenerateTournamentDetails(4);
                db.TournamentDetails.AddRange(tournaments);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Optional: Log the error
                throw new Exception($"An error occurred while seeding the database: {ex.Message}", ex);
            }
        }
        

        private static List<TournamentDetails> GenerateTournamentDetails(int nrOfTournaments)
        {
            var faker = new Faker<TournamentDetails>("sv").Rules((f, t) =>
            {

                t.Title = f.Lorem.Sentence(3);
                t.StartDate = f.Date.Future();
                t.Games = GenerateGames(f.Random.Int(min: 2, max: 10));

            });

            return faker.Generate(nrOfTournaments);

        }

        private static ICollection<Game> GenerateGames(int nrOfGames)
        {
            //string[] positions = { "Developer", "Tester", "Manager" };
            var faker = new Faker<Game>("sv").Rules((f, g) =>
            {
                g.Title = f.Lorem.Word();
                g.Time = f.Date.Future();

            });

            return faker.Generate(nrOfGames);
        }
    }
}
