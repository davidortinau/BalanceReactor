using System.Linq;
using Balance.Components;
using Balance.Controls;
using Balance.Resources.Styles;
using CommunityTemplate.Pages.Controls;
using MauiReactor;
using MauiReactor.Shapes;
using Microsoft.Maui.Devices;

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

    public List<Category> Categories { get; internal set; }

    public int SelectedCategoryIndex {get;set;}
        

    public List<string> Icons =
	[
		FluentUI.ribbon_24_regular,
		FluentUI.ribbon_star_24_regular,
		FluentUI.trophy_24_regular,
		FluentUI.badge_24_regular,
		FluentUI.book_24_regular,
		FluentUI.people_24_regular,
		FluentUI.bot_24_regular
	];

    public bool HasCompletedTasks => Tasks.Any(t => t.IsCompleted);
}

partial class ProjectDetailsPage : Component<ProjectDetailsState, ProjectDetailProps>
{
    private Project _project;

    [Inject]
    CategoryRepository _categoryRepository;

    [Inject]
    ProjectRepository _projectRepository;

    [Inject]
    TagRepository _tagRepository;

    [Inject]
    TaskRepository _taskRepository;

    protected override async void OnMounted(){
        _project = Props.Project;
        State.Name = _project.Name;
        State.Description = _project.Description;
        State.ProjectIcon = _project.Icon;
        State.CategoryID = _project.CategoryID;
        State.Tasks = _project.Tasks;
        State.Tags = _project.Tags;

        State.Categories = await _categoryRepository.ListAsync();

        State.SelectedCategoryIndex = State.Categories.IndexOf(State.Categories.FirstOrDefault(c => c.ID == State.CategoryID));

        base.OnMounted();
    }

    public override VisualNode Render()
        => ContentPage(
            ToolbarItem("Delete").OnClicked(() => DeleteProjectAsync()).IsDestructive(true),
            Grid(
                ScrollView(
                    VStack(
                        new SfTextInputLayout{
                            Entry()
                                .Text(State.Name).OnTextChanged((s, e) => SetState(s => s.Name = e.NewTextValue))
                        }
                        .Hint("Name"),
                        
                        new SfTextInputLayout
                        {
                            Entry()
                                .Text(State.Description).OnTextChanged((s, e) => SetState(s => s.Description = e.NewTextValue))
                        }
                        .Hint("Description"),

                        new SfTextInputLayout
                        {
                            Picker()
                                .ItemsSource(State.Categories.Select(c => c.Title).ToList())
                                .SelectedIndex(State.SelectedCategoryIndex)
                                .OnSelectedIndexChanged(OnCategorySelected)
                        }
                        .Hint("Category"),

                        Label("Icon")
                            .ThemeKey("Title2"),
                        CollectionView()
                            .HeightRequest(44)
                            .Margin(0, 0, 0, 15)
                            .SelectionMode(SelectionMode.Single)
                            .ItemsSource(State.Icons, RenderIcon)
                            .SelectedItem(State.ProjectIcon)
                            .ItemsLayout(new HorizontalLinearItemsLayout().ItemSpacing(ApplicationTheme.LayoutSpacing))
                            .OnSelected<MauiReactor.CollectionView,string>(OnIconSelected),

                        Label("Tags")
                            .ThemeKey("Title2"),
                        ScrollView(
                            HStack(
                                State.Tags.Select(t =>
                                    RenderChip(t)
                                ).ToArray()
                            )
                            .Spacing(ApplicationTheme.LayoutSpacing)
                            .Margin(0,0,0,15)
                            .HeightRequest(44)
                        ),                        

                        Button("Save")
                            .HeightRequest(DeviceInfo.Platform == DevicePlatform.WinUI ? 60 : 44)
                            .OnClicked(() => SaveProjectAsync()),

                        Grid(
                            Label()
                                .Text("Tasks")
                                .Style("Title2")
                                .VCenter(),
                            ImageButton()
                                .Source(ApplicationTheme.IconClean)
                                .HEnd().VCenter()
                                .Aspect(Aspect.Center)
                                .HeightRequest(44)
                                .WidthRequest(44)
                                .IsVisible(State.HasCompletedTasks)
                                .OnClicked(CleanUpTasksAsync)
                        ).HeightRequest(44),
                        VStack(
                            State.Tasks.Select(t => new TaskCard(t)).ToArray()						
                        )
                        .Spacing(ApplicationTheme.LayoutSpacing)
                            
                    )//VStack
                    .Spacing(ApplicationTheme.LayoutSpacing)
                    .Padding(ApplicationTheme.LayoutPadding)
                    
                ), //ScrollView
                new AddButton().IsTask(true)
            )//Grid
            
        );//ContentPage

    private void OnCategorySelected(int index)
    {
        SetState(s => s.CategoryID = State.Categories[index].ID);
        SetState(s => s.SelectedCategoryIndex = index);
    }

    private void OnIconSelected(string icon)
    {
        SetState(s => s.ProjectIcon = icon);
    }

    private VisualNode RenderChip(Tag t)
    {
        if(t.IsSelected)
        {
            return Border(
                Label(t.Title)
                    .TextColor(Theme.IsLightTheme ? ApplicationTheme.LightBackground : ApplicationTheme.DarkBackground)
                    .FontSize(DeviceInfo.Idiom == DeviceIdiom.Desktop ? 18 : 16)
                    .VCenter()
                    .VerticalTextAlignment(TextAlignment.Center)
            )
            .StrokeShape(new RoundRectangle().CornerRadius(22))
            .HeightRequest(44)
            .StrokeThickness(0)
            .BackgroundColor(t.DisplayColor)
            .Padding(DeviceInfo.Platform == DevicePlatform.Android ? new Thickness(18, 0, 18, 0) : DeviceInfo.Platform == DevicePlatform.WinUI ? new Thickness(18, 0, 18, 4) : new Thickness(18, 0, 18, 8))
            .OnTapped(() => ToggleTag(t));
        }else{
            return Border(
                Label(t.Title)
                    .TextColor(Theme.IsLightTheme ? ApplicationTheme.DarkOnLightBackground : ApplicationTheme.LightOnDarkBackground)
                    .FontSize(DeviceInfo.Idiom == DeviceIdiom.Desktop ? 18 : 16)
                    .VCenter()
                    .VerticalOptions(LayoutOptions.Center)
                    .VerticalTextAlignment(TextAlignment.Center)
            )
            .StrokeShape(new RoundRectangle().CornerRadius(22))
            .HeightRequest(44)
            .StrokeThickness(0)
            .BackgroundColor(Theme.IsLightTheme ? ApplicationTheme.LightSecondaryBackground : ApplicationTheme.DarkSecondaryBackground)
            .Padding(DeviceInfo.Platform == DevicePlatform.Android ? new Thickness(18, 0, 18, 0) : DeviceInfo.Platform == DevicePlatform.WinUI ? new Thickness(18, 0, 18, 4) : new Thickness(18, 0, 18, 8))
            .OnTapped(() => ToggleTag(t));
            ;
        }
    }

    private void ToggleTag(Tag t)
    {
        throw new NotImplementedException();
    }

    private async Task CleanUpTasksAsync()
    {
        var completedTasks = State.Tasks.Where(t => t.IsCompleted).ToArray();
        foreach (var task in completedTasks)
        {
            await _taskRepository.DeleteItemAsync(task);
            State.Tasks.Remove(task);
        }

        SetState(s => s.Tasks = new List<ProjectTask>(State.Tasks));
        await AppShell.DisplayToastAsync("All cleaned up!");
    }

    private async Task SaveProjectAsync()
    {
        if (_project == null)
        {
            // Handle error
            return;
        }

        _project.Name = State.Name;
        _project.Description = State.Description;
        _project.CategoryID = State.CategoryID;
        _project.Icon = State.ProjectIcon;

        await _projectRepository.SaveItemAsync(_project);

        if (_project.IsNullOrNew())
        {
            foreach (var tag in State.Tags)
            {
                if (tag.IsSelected)
                {
                    await _tagRepository.SaveItemAsync(tag, _project.ID);
                }
            }
        }

        foreach (var task in State.Tasks)
        {
            if (task.ID == 0)
            {
                task.ProjectID = _project.ID;
                await _taskRepository.SaveItemAsync(task);
            }
        }

        await Microsoft.Maui.Controls.Shell.Current.GoToAsync("..");
        await AppShell.DisplayToastAsync("Project saved");
    }

    private async Task DeleteProjectAsync()
    {
        if (_project == null)
        {
            // Handle error
            return;
        }

        await _projectRepository.DeleteItemAsync(item: _project);
        await Microsoft.Maui.Controls.Shell.Current.GoToAsync("..");
        await AppShell.DisplayToastAsync("Project deleted");
    }

    private VisualNode RenderIcon(string item)
    {
        return Grid("Auto,4","Auto",
            Label(item)
                .FontFamily("FluentUI")
                .FontSize(24)
                .Center()
                .TextColor(ApplicationTheme.LightOnDarkBackground),
            BoxView()
                .Color(ApplicationTheme.Primary)
                .HeightRequest(4)
                .HFill()
                .IsVisible(false)
                .GridRow(1)                      
        );
    }
}
