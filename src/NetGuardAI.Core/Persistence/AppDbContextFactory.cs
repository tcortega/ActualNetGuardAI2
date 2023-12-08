using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetGuardAI.Core.Persistence;

[RegisterScoped]
public class AppDbContextFactory(IConfiguration configuration) : IDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                throw new InvalidOperationException(
                                    "Could not find a connection string named 'DefaultConnection'");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString, b => b.UseNodaTime());

        return new AppDbContext(optionsBuilder.Options);
    }
}