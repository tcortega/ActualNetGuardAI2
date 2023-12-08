using Microsoft.EntityFrameworkCore;
using NetGuardAI.Core.Persistence.Entities;

namespace NetGuardAI.Core.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ScanTarget> ScanTargets { get; set; } = null!;
    public DbSet<ScanResult> ScanResults { get; set; } = null!;
    public DbSet<ScanSettings> ScanSettings { get; set; } = null!;
    public DbSet<PortRange> PortRanges { get; set; } = null!;
    public DbSet<UserWebhook> UserWebhooks { get; set; } = null!;
}
