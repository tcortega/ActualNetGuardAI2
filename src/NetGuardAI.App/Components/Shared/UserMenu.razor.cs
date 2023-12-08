using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NetGuardAI.App.Auth;
using NetGuardAI.App.Models.User;

namespace NetGuardAI.App.Components.Shared;

public partial class UserMenu
{
    [Inject] private NavigationManager Nav { get; set; }
    [Inject] private AuthenticationStateProvider Auth { get; set; }
    [Parameter] public string Class { get; set; }
    [EditorRequired] [Parameter] public UserModel User { get; set; }

    private async Task Logout()
    {
        await ((CustomAuthenticationStateProvider)Auth).Logout();
        Nav.NavigateTo("/", true);
    }
}