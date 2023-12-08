namespace NetGuardAI.Core.Hangfire;

public interface IRecurringJob
{
    Task ExecuteAsync();
}