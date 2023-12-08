namespace NetGuardAI.App.Models;

public class TicketStatisticsModel
{
    public int TotalCount { get; set; }
    public List<int> TotalLastTenDays { get; set; } = new() {20, 41, 63, 23, 38, 25, 50, 46, 11, 26};
    public double TotalIncreaseDecrease { get; set; }
    public int CompletedCount { get; set; }
    public int OngoingCount { get; set; }
    public int PendingCount { get; set; }
}