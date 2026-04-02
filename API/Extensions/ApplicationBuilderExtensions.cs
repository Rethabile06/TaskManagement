using Infrastructure.Data;

namespace API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();

            var context = scoped.ServiceProvider.GetRequiredService<TaskDbContext>();
            var logger = scoped.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("DataSeeder");

            try
            {
                await DataSeeder.SeedAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Seeding failed");
            }
        }
    }
}