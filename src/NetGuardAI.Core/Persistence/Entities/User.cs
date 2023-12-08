namespace NetGuardAI.Core.Persistence.Entities;

public class User : BaseEntity
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}