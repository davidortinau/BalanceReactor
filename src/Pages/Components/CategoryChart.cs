using Balance.Controls;
using Balance.Models;
using Balance.Resources.Styles;
using MauiReactor;
using Microsoft.Maui.Devices;

namespace Balance.Components;

partial class CategoryChart : Component
{

    [Prop]
    List<CategoryChartData>? _todoCategoryData;

    [Prop]
    List<Brush>? _todoCategoryColors;

    [Prop]
    bool _isBusy;

    public override VisualNode Render()
    {
        return Border(
            new SfShimmer
            {
                new SfCircularChart
                {
                    //Legend
                    new ChartLegend()
                    {
                        new ChartLegendLabelStyle()
                            .TextColor(Theme.IsLightTheme ? 
                                ResourceHelper.GetResource<Color>("DarkOnLightBackground") :
                                ResourceHelper.GetResource<Color>("LightOnDarkBackground"))
                            .Margin(5)
                            .FontSize(18)
                    }
                    .Placement(Syncfusion.Maui.Toolkit.LegendPlacement.Right),

                    //Series
                    new RadialBarSeries()
                        .ItemsSource(_todoCategoryData)
                        .PaletteBrushes(_todoCategoryColors ?? [])
                        .XBindingPath("Title")
                        .YBindingPath("Count")
                        .ShowDataLabels(true)
                        .EnableTooltip(true)
                        .TrackFill(Theme.IsLightTheme ?
                            ResourceHelper.GetResource<Color>("LightBackground") :
                            ResourceHelper.GetResource<Color>("DarkBackground"))
                        .CapStyle(Syncfusion.Maui.Toolkit.Charts.CapStyle.BothCurve),

                }
            }
            .CustomView(RenderCustomView())
            .IsActive(_isBusy)
            .BackgroundColor(Colors.Transparent)
            .VFill()
        )
        .Style(ResourceHelper.GetResource<Style>("CardStyle"))
        .Margin(0, 12)
        .HeightRequest(DeviceInfo.Idiom == DeviceIdiom.Phone ? 200 : 300);
    }

    static VisualNode RenderCustomView()
    {
        return Grid(
            BoxView()
                .CornerRadius(12)
                .VFill()
                .Style(ResourceHelper.GetResource<Style>("ShimmerCustomViewStyle"))
            );
    }
}