using Balance.Controls;
using Balance.Resources.Styles;
using MauiReactor;

namespace Balance.Pages;

class ProjectDetailProps
{
    public Project Project { get; set; }
}

class ProjectDetailsState 
{
    public string Name { get; set; }
    public string Description { get; internal set; }
    public int CategoryID { get; internal set; }
    public List<ProjectTask> Tasks { get; internal set; }
    public List<Tag> Tags { get; internal set; }
    public string ProjectIcon { get; internal set; }
}

class ProjectDetailsPage : Component<ProjectDetailsState, ProjectDetailProps>
{
    private Project _project;

    // public ProjectDetailsPage(Project project)
    // {
    //     _project = project;
    // }

    protected override void OnMounted(){
        _project = Props.Project;
        State.Name = _project.Name;
        State.Description = _project.Description;
        State.ProjectIcon = _project.Icon;
        State.CategoryID = _project.CategoryID;
        State.Tasks = _project.Tasks;
        State.Tags = _project.Tags;

        base.OnMounted();
    }

    // public override VisualNode Render()
    //  => ContentPage(
    //         ScrollView(
    //             VStack(
    //                 Label(State.Name)
    //                     .FontSize(32)
    //                     .HCenter()
    //             )
    //             .VCenter()
    //             .Spacing(25)
    //             .Padding(30, 0)
    //         )
    //     );

    public override VisualNode Render()
        => ContentPage(
            ScrollView(
                VStack(
                    new SfTextInputLayout{
                        Entry()
                            .Text(State.Name).OnTextChanged((s, e) => State.Name = e.NewTextValue)
                    }
                        .Hint("Name")
                        
                )
                .Spacing(ApplicationTheme.LayoutSpacing)
                .Padding(ApplicationTheme.LayoutPadding)
            )
        );
}
