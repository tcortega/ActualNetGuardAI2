namespace NetGuardAI.App.Models.Tickets;

public class TicketCommentModel
{
    public TicketCommentModel(string message, int userId, int ticketId)
    {
        Message = message;
        UserId = userId;
        TicketId = ticketId;
    }
    
    public string Message { get;  }
    public int UserId { get; }
    public int TicketId { get; }
}