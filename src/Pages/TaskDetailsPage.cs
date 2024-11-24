using System.Linq;
using Balance.Controls;
using Balance.Models;
using Balance.Resources.Styles;
using MauiReactor;
using Microsoft.Maui.Devices;

namespace Balance.Pages;

class TaskDetailsProps
{
    public ProjectTask Task { get; set; }
    
    public bool IsExistingProject { get; set; }
}

class TaskDetailsState
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public Project SelectedProject { get; set; }
    public int SelectedProjectIndex { get; set; }
    public List<Project> Projects { get; set; }
}

partial class TaskDetailsPage : Component<TaskDetailsState, TaskDetailsProps>
{
    [Inject]
    ProjectRepository _projectRepository;

    protected override async void OnMounted()
    {
        State.Projects = await _projectRepository.ListAsync();

        State.Title = Props.Task.Title;
        State.IsCompleted = Props.Task.IsCompleted;
        State.SelectedProject = State.Projects.FirstOrDefault(p => p.ID == Props.Task.ProjectID);
        State.SelectedProjectIndex = State.Projects.IndexOf(State.SelectedProject);
        

        base.OnMounted();
    }

    public override VisualNode Render()
        => ContentPage(
            ToolbarItem("Delete").OnClicked(() => DeleteTask()).IsDestructive(true),
            Grid(
                ScrollView(
                    VStack(
                        new SfTextInputLayout
                        {
                            Entry()
                                .Text(State.Title)
                                .OnTextChanged((s, e) => State.Title = e.NewTextValue)
                        }
                        .Hint("Task"),

                        new SfTextInputLayout
                        {
                            CheckBox()
                                .HorizontalOptions(LayoutOptions.End)
                                .IsChecked(State.IsCompleted)
                                .OnCheckedChanged((s, e) => State.IsCompleted = e.Value)
                        }
                        .Hint("Completed"),

                        new SfTextInputLayout
                        {
                            Picker()
                                .ItemsSource(State.Projects.Select(p => p.Name).ToList())
                                .SelectedIndex(State.SelectedProjectIndex)
                                .OnSelectedIndexChanged(index => 
                                {
                                    State.SelectedProjectIndex = index;
                                    State.SelectedProject = State.Projects[index];
                                })
                        }
                        .IsVisible(Props.IsExistingProject)
                        .Hint("Project"),

                        Button("Save")
                            .HeightRequest(DeviceInfo.Platform == DevicePlatform.WinUI ? 60 : 44)
                            .OnClicked(() => SaveTask())
                    )
                    .Spacing(ApplicationTheme.LayoutSpacing)
                    .Padding(ApplicationTheme.LayoutPadding)
                )
            )
        );

    private void DeleteTask()
    {
        throw new NotImplementedException();
    }

    private void SaveTask()
    {
        // Implement save logic here
    }
}