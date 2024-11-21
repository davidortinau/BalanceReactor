using Balance.Controls;
using Balance.Resources.Styles;
using MauiReactor;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;
using System.Linq;
using MauiControls = Microsoft.Maui.Controls;

namespace Balance;

public class AppShellState
{
    public SegmentItem[] ThemeIcons { get; set; }

    public AppTheme CurrentAppTheme {get;set;}
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

    ~AppShell()
    {
        MauiExceptions.UnhandledException -= (sender, args) =>
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
            new SegmentItem().ImageSource("IconLight"),
            new SegmentItem().ImageSource("IconDark")
        };

        State.ThemeIcons = themeIcons;
        State.CurrentAppTheme = Application.Current.UserAppTheme;
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
        )
        .FlyoutFooter(
            Grid(  
                HStack(
                    RadioButton()
                        .Content("Light")
                        .Value(AppTheme.Light)
                        .IsChecked(State.CurrentAppTheme == AppTheme.Light)
                        .OnCheckedChanged(checkedAction:()=>{
                            Application.Current.UserAppTheme = AppTheme.Light;
                        }),
                    RadioButton()
                        .Content("Dark")
                        .Value(AppTheme.Dark)
                        .IsChecked(State.CurrentAppTheme == AppTheme.Dark)
                        .OnCheckedChanged(checkedAction:()=>{
                            Application.Current.UserAppTheme = AppTheme.Dark;
                        })
                ).Spacing(10)            
                // new SegmentedControl
                //     {
                //      State.ThemeIcons
                //     }
                //     .SegmentWidth(40).SegmentHeight(40)

                //     .OnSelectionChanged((object? sender, SelectionChangedEventArgs args) =>
                //     {
                //         // SetState(s => s.CurrentAppTheme = args.CurrentSelection as SegmentItem == State.ThemeIcons[0] ? AppTheme.Light : AppTheme.Dark);
                //         Application.Current.UserAppTheme = args.CurrentSelection as SegmentItem == State.ThemeIcons[0] ? AppTheme.Light : AppTheme.Dark;

                        
                //     })
                    
            )
            .Padding(15)
        );
}

