using Balance.Pages.Controls;
using Balance.Resources.Styles;
using MauiReactor;
using System.Diagnostics;

namespace Balance;

public class AppShellState
{
    public string[] ThemeIcons { get; set; }
}

public class AppShell : Component<AppShellState>
{
    public AppShell()
    {
        MauiExceptions.UnhandledException += (sender, args) =>
        {
            Debug.WriteLine(args.ExceptionObject);
            throw (Exception)args.ExceptionObject;
        };
    }

    protected override void OnMounted()
    {
        base.OnMounted();

        var themeIcons = new[]
        {
            "IconLight",
            "IconDark"
        };

        State.ThemeIcons = themeIcons;
    }
    public override VisualNode Render()
        => Shell(
            FlyoutItem("Dashboard",
                ShellContent()
                    .Title("Dashboard")
                    .FlyoutIcon(ApplicationTheme.IconDashboard)   
                    .Icon(ApplicationTheme.IconDashboard)
                    .RenderContent(() => new MainPage())
                    .Route("main")
            ),
            FlyoutItem("Projects",
                ShellContent()
                    .Title("Projects")
                    // .Icon((FontImageSource)Application.Current.Resources["IconProjects"])
                    .RenderContent(() => new ProjectListPage())
                    .Route("projects")
            ),
            FlyoutItem("Manage Meta",
                ShellContent()
                    .Title("Manage Meta")
                    // .Icon((FontImageSource)Application.Current.Resources["IconMeta"])
                    .RenderContent(() => new ManageMetaPage())
                    .Route("manage")
            )
        );
        // .FlyoutFooter(
        //     Grid(              
        //         new SegmentedControl()
        //             .SegmentWidth(40).SegmentHeight(40)
        //             .ItemSource(State.ThemeIcons.Select(icon => new SfSegmentItem { ImageSource = icon }).ToList())
        //     )
        //     .Padding(15)
        // );
}

