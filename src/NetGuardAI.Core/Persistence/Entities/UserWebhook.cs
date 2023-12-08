namespace NetGuardAI.Core.Persistence.Entities;

public class UserWebhook : BaseEntity
{
    public required string Url { get; set; }
}