using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Esm.Infrastructure.Persistence;

public class EsmDbContextInitializer(ILogger<EsmDbContextInitializer> logger, EsmDbContext context)
{
    public async Task InitializeAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
        }
    }
}