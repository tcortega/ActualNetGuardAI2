using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace NetGuardAI.App.Components.Shared.Themes;

public partial class ThemesButton
{
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
}