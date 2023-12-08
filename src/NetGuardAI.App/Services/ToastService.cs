using MudBlazor;

namespace NetGuardAI.App.Services;

public class ToastService
{
    private readonly ISnackbar _snackbar;

    public ToastService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void InvalidTicket(int id)
    {
        _snackbar.Add($"The ticket #{id} does not exist. Maybe refresh the page to see the updated list?", Severity.Error);
    }

    public void ActionError(string action, int id)
    {
        _snackbar.Add($"You're not able to {action} the ticket {id} due to it's status", Severity.Error);
    }

    public void Success(string message)
    {
        _snackbar.Add(message, Severity.Success);
    }

    public void ActionSuccess(string action, int id)
    {
        _snackbar.Add($"Successfully {action} the ticket {id}", Severity.Success);
    }

    public void InvalidComment()
    {
        _snackbar.Add("The comment is invalid. It must contain at least 5 characters", Severity.Error);
    }

    public void CommentSuccess()
    {
        _snackbar.Add("Successfully sent a new comment", Severity.Success);
    }

    public void Error(string message)
    {
        _snackbar.Add(message, Severity.Error);
    }
}