using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NetGuardAI.App.Components.Shared;

public partial class CommandPalette
{
    private readonly Dictionary<string, string> _pages = new();
    private Dictionary<string, string> _pagesFiltered = new();
    private string _search;
    [Inject] private NavigationManager Navigation { get; set; }
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    protected override void OnInitialized()
    {
        _pages.Add("App", "/");

        _pagesFiltered = _pages;
    }

    private void SearchPages(string value)
    {
        _pagesFiltered = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(value))
            _pagesFiltered = _pages
                .Where(x => x.Key
                    .Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value);
        else
            _pagesFiltered = _pages;
    }
}