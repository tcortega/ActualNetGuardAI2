using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using NetGuardAI.App.Models.Charts;
using NetGuardAI.App.Models.Charts.Chart;
using NetGuardAI.App.Models.Charts.DataLabels;
using NetGuardAI.App.Models.Charts.PlotOptions;
using NetGuardAI.App.Models.Charts.Series;
using NetGuardAI.App.Models.Charts.Stroke;
using NetGuardAI.App.Models.Charts.Tooltip;
using NetGuardAI.App.Models.Charts.YAxis;

namespace NetGuardAI.App.Components.Cards;

public partial class DataCard : MudComponentBase
{
    private ChartOptionsModel<SeriesModel<int>, int>? _chartOptions = null;

    private string Classname =>
        new CssBuilder()
            .AddClass(Class)
            .Build();

    [EditorRequired] [Parameter] public string Title { get; set; } = string.Empty;
    [EditorRequired] [Parameter] public string ChartId { get; set; } = string.Empty;
    [Parameter] public string BarColor { get; set; } = "var(--mud-palette-primary)";
    [Parameter] public double? IncreaseDecrease { get; set; }
    [EditorRequired] [Parameter] public int Total { get; set; }
    [EditorRequired] [Parameter] public List<int> LastTenDays { get; set; } = new();

    private string _positiveBackgroundColor = "background: rgba(84, 214, 44, 0.16);";
    private string _negativeBackgroundColor = "background: rgba(255, 72, 66, 0.16);";

    protected override void OnInitialized()
    {
        _chartOptions = new ChartOptionsModel<SeriesModel<int>, int>
        {
            Chart = new ChartModel
            {
                Type = ChartTypes.Bar,
                Width = "100px",
                Sparkline = new ChartModel.SparklineModel
                {
                    Enabled = true
                },
                Toolbar = new ChartModel.ToolbarModel
                {
                    Show = false
                },
                Id = ChartId,
            },
            Colors = new List<string> {BarColor},
            Series = new List<SeriesModel<int>>
            {
                new()
                {
                    Name = "LastTenDays",
                    Data = LastTenDays
                }
            },
            Stroke = new StrokeModel
            {
                Show = false
            },
            PlotOptions = new PlotOptionsModel
            {
                Bar = new PlotOptionsModel.BarModel
                {
                    BorderRadius = 2,
                    ColumnWidth = "80%"
                }
            },
            Tooltip = new TooltipModel
            {
                Enabled = false
            },
            DataLabels = new DataLabelsModel
            {
                Enabled = false
            },
            YAxis = new YAxisModel()
            {
                Max = 50
            }
        };
    }
}