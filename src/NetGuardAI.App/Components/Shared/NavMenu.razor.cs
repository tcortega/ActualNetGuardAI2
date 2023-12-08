using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NetGuardAI.App.Models;
using NetGuardAI.App.Models.User;

namespace NetGuardAI.App.Components.Shared;

public partial class NavMenu
{
    [EditorRequired] [Parameter] public ThemeManagerModel ThemeManager { get; set; }
    [EditorRequired] [Parameter] public bool CanMiniSideMenuDrawer { get; set; }
    [EditorRequired] [Parameter] public EventCallback ToggleSideMenuDrawer { get; set; }
    [EditorRequired] [Parameter] public EventCallback OpenCommandPalette { get; set; }
    [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }

    private UserModel _user = new()
    {
        UserName = string.Empty
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var user = (await AuthenticationState).User;
        var claims = user.Claims;

        _user.UserName = claims.First(x => x.Type == ClaimTypes.Name).Value;
        // _user.Email = claims.First(x => x.Type == ClaimTypes.Email).Value;
    }
}