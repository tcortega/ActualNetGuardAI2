using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetGuardAI.Core.Hangfire;
using NetGuardAI.Core.Persistence;
using NetGuardAI.Masscan;
using NetGuardAI.Nmap;
using NodaTime;
using OpenAI.Extensions;

namespace NetGuardAI.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddHangfire(services, configuration);

        services.AutoRegister();
        services.AddAsyncInitialization();

        services.AddSingleton<MasscanWrapper>();
        services.AddSingleton<NmapWrapper>();
        services.AddSingleton<IClock>(SystemClock.Instance);
        services.AddOpenAIService();
        

        return services;
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException(
                                   "Could not find a connection string named 'DefaultConnection'");

        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(connectionString, b => b.UseNodaTime())
        );
    }

    private static void AddHangfire(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Hangfire") ??
                               throw new InvalidOperationException(
                                   "Could not find a connection string named 'Hangfire'");

        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(connectionString, b => b.UseNodaTime())
        );

        services.AddHangfire(cfg =>
        {
            Dapper.SqlMapper.AddTypeHandler(new DapperDateTimeTypeHandler()); //
            cfg.UseRecommendedSerializerSettings();
            cfg.UsePostgreSqlStorage(opt => 
                opt.UseNpgsqlConnection(connectionString));
        });

        services.AddHangfireServer();
    }
}