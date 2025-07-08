using Torunament.Data.Data;
using Domain.Presentation.Data;

namespace Domain.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TournamentApiContext>();

            //// Apply any pending migrations
            //await context.Database.MigrateAsync();

            // Seed the database
            await SeedData.SeedAsync(context);
        }
    }
}
