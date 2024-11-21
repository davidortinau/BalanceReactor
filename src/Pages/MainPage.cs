using Balance.Resources.Styles;
using MauiReactor;

namespace Balance.Pages;

public class MainPageState
{
    public int Counter { get; set; }
}

public class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
     => ContentPage(
            ScrollView(
                VStack(
                    Image()
                        .Source(ApplicationTheme.IconDashboard)
                        .HeightRequest(200)
                        .HCenter()
                        .Set(Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty, "Cute dot net bot waving hi to you!"),

                    Label("Hello, David!")
                        .FontSize(32)
                        .HCenter(),

                    Label("Welcome to MauiReactor: .NET MAUI with superpowers!")
                        .FontSize(18)
                        .HCenter(),

                    Button(State.Counter == 0 ? "Click me" : $"Clicked {State.Counter} times!")
                        .OnClicked(()=>SetState(s => s.Counter ++))
                        .HCenter()
                        .BackgroundColor(Color.FromArgb("#FF6600"))
                        .TextColor(Colors.White)
                        .CornerRadius(12)
                        .HeightRequest(44)
                )
                .VCenter()
                .Spacing(25)
                .Padding(30, 0)
            )
        );
}