using System.ComponentModel;

namespace NetGuardAI.App.Models.SideMenu;

public enum PageStatus
{
    [Description("Coming Soon")] ComingSoon,
    [Description("WIP")] Wip,
    [Description("New")] New,
    [Description("CompletedCount")] Completed,
    [Description("Hidden")] Hidden
}