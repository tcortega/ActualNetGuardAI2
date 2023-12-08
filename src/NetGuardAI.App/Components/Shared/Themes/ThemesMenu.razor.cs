using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetGuardAI.App.Models;

namespace NetGuardAI.App.Components.Shared.Themes;

public partial class ThemesMenu
{
    // TODO: The razor file could probably be cleaner..
    // TODO: Find why MudOverlay does not work..
    // TODO: Better CSS..
    
    private readonly List<string> _primaryColors = new()
    {
        Colors.Green.Default,
        Colors.Blue.Default,
        Colors.BlueGrey.Default,
        Colors.Purple.Default,
        Colors.Orange.Default,
        Colors.Red.Default
    };

    [EditorRequired] [Parameter] public bool ThemingDrawerOpen { get; set; }
    [EditorRequired] [Parameter] public EventCallback<bool> ThemingDrawerOpenChanged { get; set; }
    [EditorRequired] [Parameter] public ThemeManagerModel ThemeManager { get; set; }
    [EditorRequired] [Parameter] public EventCallback<ThemeManagerModel> ThemeManagerChanged { get; set; }

    private async Task UpdateThemePrimaryColor(string color)
    {
        ThemeManager.PrimaryColor = color;
        await ThemeManagerChanged.InvokeAsync(ThemeManager);
    }
    
    private async Task ToggleDarkLightMode(bool isDarkMode)
    {
        ThemeManager.IsDarkMode = isDarkMode;
        await ThemeManagerChanged.InvokeAsync(ThemeManager);
    }
}