using System.Linq;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;

namespace Balance.Components;

public partial class ProjectCard : Component
{
    [Prop]
    double _width;
    
    [Prop]
    Project _project;

    [Inject]
    ILogger<ProjectCard> _logger;

    public override VisualNode Render()
    => Border(
        VStack(
            Image()
                        .HStart()
                        .Aspect(Aspect.Center)
                        .Source( new FontImageSource {
                            Glyph = _project.Icon, // Replace with actual glyph
                            FontFamily = FluentUI.FontFamily,
                            Color = Theme.IsLightTheme ? ApplicationTheme.DarkOnLightBackground : ApplicationTheme.LightOnDarkBackground,
                            Size = ApplicationTheme.IconSize}),
                    Label(_project.Name).TextColor(ApplicationTheme.Gray400).FontSize(14).TextTransform(TextTransform.Uppercase),
                    Label(_project.Description).LineBreakMode(LineBreakMode.WordWrap),
                    HStack(
                        _project.Tags.Select(t =>
                        Border(
                            Label(t.Title).TextColor(ApplicationTheme.LightBackground).FontSize(14).VCenter()
                        )
                        .StrokeShape(new RoundRectangle().CornerRadius(16))
                        .HeightRequest(32)
                        .StrokeThickness(0)
                        .Background(Theme.IsLightTheme ? t.DisplayLightColor : t.DisplayDarkColor)
                        .Padding(12,0,12,8)).ToArray()
                    ).Spacing(15)
        )
        .Spacing(15)
    )
    .OnTapped(() => NavigateToProject(_project))
    .WidthRequest(_width)
    .ThemeKey("CardStyle");

    private async void NavigateToProject(Project project)
	{
		try{
			await Microsoft.Maui.Controls.Shell.Current.GoToAsync<ProjectDetailProps>(
			nameof(ProjectDetailsPage),
			props =>
				{
					props.Project = project;
				}
			);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error navigating to project details");
		}
	}
}