using MauiReactor;

namespace Balance.Pages;

class ManageMetaPageState
{
    public int Counter { get; set; }
}

class ManageMetaPage : Component<ManageMetaPageState>
{
    public override VisualNode Render()
     => ContentPage(
            ScrollView(
                VStack(
                    Label("Manage Meta")
                        .FontSize(32)
                        .HCenter()
                )
                .VCenter()
                .Spacing(25)
                .Padding(30, 0)
            )
        );
}