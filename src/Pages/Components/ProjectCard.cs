using System.Linq;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;

namespace Balance.Components;

public class ProjectCardState
{
    public string Name {get;set;}
    public string Description {get;set;}
    public List<Tag> Tags { get; internal set; }
    public string Icon {get; internal set;}
}

public partial class ProjectCard : Component<ProjectCardState>
{
    [Prop]
    double _width;
    private Project p;


    public ProjectCard(Project p)
    {
        this.p = p;
    }

    protected override void OnMounted()
    {
        State.Name = p.Name;
        State.Description = p.Description;
        State.Tags = p.Tags;
        State.Icon = p.Icon;
        base.OnMounted();
    }

    public override VisualNode Render()
    => Border(
        VStack(
            Image()
                        .HStart()
                        .Aspect(Aspect.Center)
                        .Source( new FontImageSource {
                            Glyph = State.Icon, // Replace with actual glyph
                            FontFamily = FluentUI.FontFamily,
                            Color = Theme.IsLightTheme ? ApplicationTheme.DarkOnLightBackground : ApplicationTheme.LightOnDarkBackground,
                            Size = ApplicationTheme.IconSize}),
                    Label(State.Name).TextColor(ApplicationTheme.Gray400).FontSize(14).TextTransform(TextTransform.Uppercase),
                    Label(State.Description).LineBreakMode(LineBreakMode.WordWrap),
                    HStack(
                        State.Tags.Select(t =>
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
    .WidthRequest(_width)
    .ThemeKey("CardStyle");
}