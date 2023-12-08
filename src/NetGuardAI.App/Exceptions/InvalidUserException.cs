namespace NetGuardAI.App.Exceptions;

public class InvalidUserException : Exception
{
    public InvalidUserException()
        : base("The provided user is invalid. Please retry your login.")
    {
    }
}