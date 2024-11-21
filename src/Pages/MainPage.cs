using Balance.Components;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;

namespace Balance.Pages;

public class MainPageState
{
    public int Counter { get; set; }
}

public class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
     => ContentPage(
            Grid(
                VScrollView(
                    VStack(
                        Label("Projects").ThemeKey("Title2"),
                        HScrollView(
                            HStack(
                                new ProjectCard().Width(200)
                            )
                            .Spacing(15)
                            .Padding(30,0)
                        )
                        .Margin(-30,0)
                    )
                    .Spacing(ApplicationTheme.LayoutSpacing)
                    .Padding(ApplicationTheme.LayoutPadding)
                )
            )
        ).Title(DateTime.Now.ToString("dddd, MMM d"));
}