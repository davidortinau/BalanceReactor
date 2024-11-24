using MauiReactor;
using Syncfusion.Maui.Toolkit.Hosting;
using CommunityToolkit.Maui;
using Balance.Resources.Styles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using MauiControls = Microsoft.Maui.Controls;


namespace Balance;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
#if DEBUG
            .EnableMauiReactorHotReload()
#endif
            .ConfigureSyncfusionToolkit()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
				fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
            });

#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddLogging(configure => configure.AddDebug());
        builder.OnMauiReactorUnhandledException(ex =>
        {
            System.Diagnostics.Debug.WriteLine(ex);
        });
#endif
        builder.Services.AddSingleton<ProjectRepository>();
		builder.Services.AddSingleton<TaskRepository>();
		builder.Services.AddSingleton<CategoryRepository>();
		builder.Services.AddSingleton<TagRepository>();
		builder.Services.AddSingleton<SeedDataService>();
		// builder.Services.AddSingleton<ModalErrorHandler>();

        
        MauiReactor.Routing.RegisterRoute<ProjectDetailsPage>(nameof(ProjectDetailsPage));
        MauiReactor.Routing.RegisterRoute<ManageMetaPage>(nameof(ManageMetaPage));
        MauiReactor.Routing.RegisterRoute<TaskDetailsPage>(nameof(TaskDetailsPage));
		// builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");

        return builder.Build();
    }
}