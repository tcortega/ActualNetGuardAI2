using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NetGuardAI.App.Models.SideMenu;
using NetGuardAI.App.Models.User;

namespace NetGuardAI.App.Components.Shared;

public partial class SideMenu
{
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

    private List<MenuSectionModel> _menuSections = new()
    {
        new MenuSectionModel
        {
            Title = "GERAL",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Dashboard",
                    Icon = Icons.Material.Filled.Home,
                    Href = "/"
                },
                new()
                {
                    Title = "Configurações",
                    Icon = Icons.Material.Filled.Settings,
                    Href = "/settings"
                },
                new()
                {
                    Title = "Webhooks",
                    Icon = Icons.Material.Filled.NotificationsActive,
                    Href = "/webhooks"
                }
            }
        },

        // new MenuSectionModel
        // {
        //     Title = "CONFIGURAÇÕES",
        //     SectionItems = new List<MenuSectionItemModel>
        //     {
        //         new()
        //         {
        //             IsParent = true,
        //             Title = "Tickets",
        //             Icon = Icons.Material.Filled.Article,
        //             MenuItems = new()
        //             {
        //                 new MenuSectionSubItemModel
        //                 {
        //                     Title = "List",
        //                     Href = "/tickets",
        //                     PageStatus = PageStatus.Completed
        //                 }
        //             }
        //         }
        //     }
        // }
    };

    [EditorRequired] [Parameter] public bool SideMenuDrawerOpen { get; set; }
    [EditorRequired] [Parameter] public EventCallback<bool> SideMenuDrawerOpenChanged { get; set; }
    [EditorRequired] [Parameter] public bool CanMiniSideMenuDrawer { get; set; }
    [EditorRequired] [Parameter] public EventCallback<bool> CanMiniSideMenuDrawerChanged { get; set; }
}