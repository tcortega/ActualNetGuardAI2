using System.Reflection;
using Extensions.Hosting.AsyncInitialization;
using Hangfire;

namespace NetGuardAI.Core.Hangfire;

[RegisterTransient]
public class HangfireInitializer(IRecurringJobManager recurringJobManager) : IAsyncInitializer
{
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        var initializeJobMethod = GetType()
            .GetMethod(nameof(InitializeJob), BindingFlags.NonPublic | BindingFlags.Static);
        ArgumentNullException.ThrowIfNull(initializeJobMethod);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var targetAssembly = typeof(CoreDependencyInjection).Assembly.FullName;
            if (assembly.FullName != targetAssembly) continue;
            
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass)
                    continue;

                if (type.GetCustomAttribute<RecurringJobAttribute>() is not { } rja)
                    continue;

                var func = initializeJobMethod.MakeGenericMethod(type);
                func.Invoke(null, new object[] { recurringJobManager, rja.RecurringJobId, rja.Cron, rja.TimeZone, rja.Queue, });
            }
        }

        return Task.CompletedTask;
    }

    private static void InitializeJob<T>(IRecurringJobManager jobManager, string jobid, string cron, string timezone,
        string queue)
        where T : class, IRecurringJob
    {
        jobManager.AddOrUpdate<T>(
            jobid,
            queue,
            t => t.ExecuteAsync(),
            cron,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone),
            });
    }
}