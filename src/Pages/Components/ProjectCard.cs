using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;

namespace Balance.Components;

public class ProjectCardState
{
    public string Name {get;set;}
}

public partial class ProjectCard : Component<ProjectCardState>
{
    [Prop]
    double _width;

    protected override void OnMounted()
    {
        State.Name = "Project Reactor";
        base.OnMounted();
    }

    public override VisualNode Render()
    => Border(
        VStack(
            Image()
                        .HStart()
                        .Aspect(Aspect.Center)
                        .Source(ApplicationTheme.IconRibbon),
                    Label(State.Name).TextColor(ApplicationTheme.Gray400).FontSize(14).TextTransform(TextTransform.Uppercase),
                    Label("Some words about this project").LineBreakMode(LineBreakMode.WordWrap),
                    HStack(
                        Border(
                            Label("Personal").TextColor(ApplicationTheme.LightBackground).FontSize(14).VCenter()
                        )
                        .StrokeShape(new RoundRectangle().CornerRadius(16))
                        .HeightRequest(32)
                        .StrokeThickness(0)
                        .Background(ApplicationTheme.Primary)
                        .Padding(12,0,12,8)
                    ).Spacing(15)
        )
        .Spacing(15)
    )
    .WidthRequest(_width)
    .ThemeKey("CardStyle");
}