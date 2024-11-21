using MauiReactor;
using Microsoft.Extensions.Logging;
using Balance.Pages;
using Syncfusion.Maui.Toolkit.Hosting;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Hosting;
using Balance.Resources.Styles;


namespace Balance;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiReactorApp<AppShell>(app =>{
                app.UseTheme<ApplicationTheme>();
            })
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
#endif

        return builder.Build();
    }
}