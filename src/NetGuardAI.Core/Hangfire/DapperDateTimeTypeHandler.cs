using System.Data;
using Dapper;

namespace NetGuardAI.Core.Hangfire;

public class DapperDateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override DateTime Parse(object value)
    {
        return value switch
        {
            DateTime dateTime => dateTime,
            NodaTime.Instant i => i.ToDateTimeUtc(),
            NodaTime.LocalDate ld => ld.ToDateTimeUnspecified(),
            _ => throw new ArgumentException(
                $"Invalid value of type '{value?.GetType().FullName}' given. DateTime or NodaTime.Instant values are supported.",
                nameof(value))
        };
    }

    public void SetValue(IDbDataParameter parameter, object value)
        => parameter.Value = value;

    public override void SetValue(IDbDataParameter parameter, DateTime value)
        => parameter.Value = value;
}