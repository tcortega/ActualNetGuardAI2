namespace NetGuardAI.App.Exceptions;

public class InvalidTicketException : Exception
{
    public InvalidTicketException()
        : base("The provide ticket is invalid.")
    {
    }
}