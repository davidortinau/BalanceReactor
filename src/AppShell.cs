using Balance.Controls;
using Balance.Resources.Styles;
using MauiReactor;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;

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
                    .RenderContent(() => new MainPage())
                    .Route("main")
            ).Icon(()=> ApplicationTheme.IconDashboard),
            FlyoutItem("Projects",
                ShellContent()
                    .Title("Projects")
                    .RenderContent(() => new ProjectListPage())
                    .Route("projects")
            ).Icon(()=> ApplicationTheme.IconProjects),
            FlyoutItem("Manage Meta",
                ShellContent()
                    .Title("Manage Meta")
                    .RenderContent(() => new ManageMetaPage())
                    .Route("manage")
            ).Icon(()=> ApplicationTheme.IconMeta)
        )
        .FlyoutFooter(
            Grid(  
                new SfSegmentedControl
                {
                    new SfSegmentItem()
                        .Text("Light")
                        .ImageSource(ResourceHelper.GetResource<FontImageSource>("IconLight")),
                    new SfSegmentItem()
                        .Text("Dark")
                        .ImageSource(ResourceHelper.GetResource<FontImageSource>("IconDark"))
                }
                .SelectedIndex(Theme.CurrentAppTheme == AppTheme.Light ? 0 : 1)
                .OnSelectionChanged((s, e) => Theme.UserTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark)
                .VerticalOptions(LayoutOptions.Center)
                .HorizontalOptions(LayoutOptions.Center)
                .SegmentWidth(40)
                .SegmentHeight(40)
                // HStack(
                //     RadioButton()
                //         .Content("Light")
                //         .Value(AppTheme.Light)
                //         .IsChecked(State.CurrentAppTheme == AppTheme.Light)
                //         .OnCheckedChanged(checkedAction:()=>{
                //             Application.Current.UserAppTheme = AppTheme.Light;
                //         }),
                //     RadioButton()
                //         .Content("Dark")
                //         .Value(AppTheme.Dark)
                //         .IsChecked(State.CurrentAppTheme == AppTheme.Dark)
                //         .OnCheckedChanged(checkedAction:()=>{
                //             Application.Current.UserAppTheme = AppTheme.Dark;
                //         })
                // ).Spacing(10)            
                
                    
            )
            .Padding(15)
        );
}

