using MauiReactor;
using Syncfusion.Maui.Toolkit.Hosting;
using CommunityToolkit.Maui;
using Balance.Resources.Styles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using MauiControls = Microsoft.Maui.Controls;
using MauiReactor.HotReload;


namespace Balance;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
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
        builder.EnableMauiReactorHotReload();
        builder.OnMauiReactorUnhandledException(ex =>
        {
            System.Diagnostics.Debug.WriteLine(ex);
        });
#endif
        
        
        MauiReactor.Routing.RegisterRoute<ProjectDetailsPage>(nameof(ProjectDetailsPage));
        MauiReactor.Routing.RegisterRoute<ManageMetaPage>(nameof(ManageMetaPage));
        MauiReactor.Routing.RegisterRoute<TaskDetailsPage>(nameof(TaskDetailsPage));
		// builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");

        RegisterServices(builder.Services);

        return builder.Build();
    }

    [ComponentServices]
    static void RegisterServices(IServiceCollection services)
    {
#if DEBUG
        services.AddLogging(configure => configure.AddDebug());
#endif

        services.AddSingleton<ProjectRepository>();
        services.AddSingleton<TaskRepository>();
        services.AddSingleton<CategoryRepository>();
        services.AddSingleton<TagRepository>();
        services.AddSingleton<SeedDataService>();
        services.AddSingleton<ModalErrorHandler>();
    }
}