using Balance.Controls;
using Balance.Resources.Styles;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiReactor;
using Microsoft.Maui.ApplicationModel;
using System.Diagnostics;
using System.Linq;
using Font = Microsoft.Maui.Font;
using MauiControls = Microsoft.Maui.Controls;

namespace Balance;

public class AppShellState
{

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

        State.CurrentAppTheme = Application.Current.UserAppTheme;
    }
    public override VisualNode Render()
        => Shell(
            FlyoutItem("Dashboard",
                ShellContent()
                    .Title("Dashboard")
                    .RenderContent(() => new MainPage())
                    .Route("main")
            ).Icon(ResourceHelper.GetResource<FontImageSource>("IconDashboard")),
            FlyoutItem("Projects",
                ShellContent()
                    .Title("Projects")
                    .RenderContent(() => new ProjectsPage())
                    .Route("projects")
            ).Icon(ResourceHelper.GetResource<FontImageSource>("IconProjects")),
            FlyoutItem("Manage Meta",
                ShellContent()
                    .Title("Manage Meta")
                    .RenderContent(() => new ManageMetaPage())
                    .Route("manage")
            ).Icon(ResourceHelper.GetResource<FontImageSource>("IconMeta"))
        )
        .FlyoutFooter(
            Grid(            
                new SfSegmentedControl{
                    new SfSegmentItem().ImageSource(ResourceHelper.GetResource<FontImageSource>("IconLight")),
                    new SfSegmentItem().ImageSource(ResourceHelper.GetResource<FontImageSource>("IconDark"))
                }
                .Background(Colors.Transparent)
                .ShowSeparator(true)
                .SegmentCornerRadius(0)
                .Stroke(Theme.IsLightTheme ? ApplicationTheme.Black : ApplicationTheme.White)
                .StrokeThickness(1)
                .SelectedIndex(Theme.CurrentAppTheme == AppTheme.Light ? 0 : 1)
                .OnSelectionChanged((s, e) => Theme.UserTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark)
                .SegmentWidth(40)
                .SegmentHeight(40)
                    
            )
            .Padding(15)
        );

    public static async Task DisplaySnackbarAsync(string message)
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		var snackbarOptions = new SnackbarOptions
		{
			BackgroundColor = Color.FromArgb("#FF3300"),
			TextColor = Colors.White,
			ActionButtonTextColor = Colors.Yellow,
			CornerRadius = new CornerRadius(0),
			Font = Font.SystemFontOfSize(18),
			ActionButtonFont = Font.SystemFontOfSize(14)
		};

		var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

		await snackbar.Show(cancellationTokenSource.Token);
	}

	public static async Task DisplayToastAsync(string message)
	{
		// Toast is currently not working in MCT on Windows
		if (OperatingSystem.IsWindows())
			return;

		var toast = Toast.Make(message, textSize: 18);

		var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
		await toast.Show(cts.Token);
	}
}

