using MauiReactor;

namespace Balance.Pages;

class ProjectListPageState
{
    public int Counter { get; set; }
}

class ProjectListPage : Component<ProjectListPageState>
{
    public override VisualNode Render()
     => ContentPage(
            ScrollView(
                VStack(
                    Label("Project List")
                        .FontSize(32)
                        .HCenter()
                )
                .VCenter()
                .Spacing(25)
                .Padding(30, 0)
            )
        );
}