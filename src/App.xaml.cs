using Balance.Resources.Styles;
using MauiReactor;

namespace Balance;

public partial class App : MauiReactorApplication
{
    public App(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        InitializeComponent();
    }
}


public abstract class MauiReactorApplication : ReactorApplication<AppShell>
{
    public MauiReactorApplication(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        this.UseTheme<ApplicationTheme>();
    }
}