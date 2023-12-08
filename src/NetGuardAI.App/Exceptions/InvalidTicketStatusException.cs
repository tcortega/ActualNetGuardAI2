namespace NetGuardAI.App.Exceptions;

public class InvalidTicketStatusException : Exception
{
    public InvalidTicketStatusException(string status) : 
        base($"The ticket is not {status}. Therefore it is not possible to perform this action.")
    {
    }
}