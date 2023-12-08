using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using NetGuardAI.Core.Persistence.Entities;
using NodaTime;

namespace NetGuardAI.Core.Persistence;

[RegisterTransient]
public class DbInitializer(AppDbContext context) : IAsyncInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (context.Database.IsNpgsql())
        {
            await context.Database.MigrateAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        var hasUsers = await context.Users.AnyAsync(cancellationToken);
        if (!hasUsers)
        {
            var user = new User { UserName = "Admin", Password = "123123"};
            context.Users.Add(user);
        }

        var hasPortRanges = await context.PortRanges.AnyAsync(cancellationToken);
        if (!hasPortRanges)
        {
            var settings = new PortRange { FromPort = 1, ToPort = 65535 };
            context.PortRanges.Add(settings);
        }
        
        var hasSettings = await context.ScanSettings.AnyAsync(cancellationToken);
        if (!hasSettings)
        {
            var settings = new ScanSettings { 
                MasscanRate = 1000, NmapConcurrencyLimit = 100, IpCooldown = Duration.FromHours(6)
            };
            context.ScanSettings.Add(settings);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}