﻿using System.Linq;
using Balance.Components;
using Balance.Resources.Styles;
using CommunityTemplate.Pages.Controls;
using MauiReactor;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;
using MauiControls = Microsoft.Maui.Controls;

namespace Balance.Pages;

class MainPageState
{
    public bool IsBusy { get; set; }

    public bool IsRefreshing { get; set; }

    public List<CategoryChartData> TodoCategoryData {get;set;} = [];

	public List<Brush> TodoCategoryColors {get;set;} = [];

	public List<ProjectTask> Tasks {get;set;} = [];

	public List<Project> Projects {get;set;} = [];

	public bool IsNavigatedTo {get;set;}

	public bool IsDataLoaded {get;set;}

	public bool HasCompletedTasks {get;set;}
    
}

partial class MainPage : Component<MainPageState>
{
    [Inject]
    ProjectRepository _projectRepository;

    [Inject]
    CategoryRepository _categoryRepository;

    [Inject]
    TaskRepository _taskRepository;

    [Inject]
    SeedDataService _seedDataService;

	[Inject]
	ILogger<MainPage> _logger;

	[Inject]
    ModalErrorHandler _errorHandler;

	public override VisualNode Render()
	{
      return ContentPage(DateTime.Now.ToString("dddd, MMM d"),
            Grid(
                VScrollView(
                    VStack(
                        new CategoryChart()
                            .IsBusy(State.IsBusy)
                            .TodoCategoryData(State.TodoCategoryData)
                            .TodoCategoryColors(State.TodoCategoryColors),

                        Label("Projects").ThemeKey("Title2"),
                        HScrollView(
                            HStack(
								State.Projects.Select(p => 
									new ProjectCard()
										.Project(p)
										.Width(200)
										
								).ToArray()
                            )
                            .Spacing(15)
                            .Padding(30,0)
                        )
                        .Margin(-30,0),
						Grid(
							Label("Tasks").ThemeKey("Title2").VCenter(),
							ImageButton()
								.Source(ApplicationTheme.IconClean)
								.IsVisible(State.HasCompletedTasks)
								.HEnd().VCenter()
								.OnClicked(CleanUpTasksAsync)
						).HeightRequest(44),
						VStack(
							State.Tasks.Select(t => new TaskCard().Task(t).OnCompletedChanged(OnTaskCompletedChanged)).ToArray()
						).Spacing(15)
                    )
                    .Spacing(ApplicationTheme.LayoutSpacing)
                    .Padding(ApplicationTheme.LayoutPadding)
                ),
				new AddButton().IsTask(true)
            )
        )
		.OnNavigatedTo(() => State.IsNavigatedTo = true)
		.OnNavigatedFrom(() => State.IsNavigatedTo = false)
		.OnAppearing(OnAppearingAsync);
	}

    private async Task OnTaskCompletedChanged(ProjectTask task, bool isCompleted)
    {
        await _taskRepository.SaveItemAsync(task);
		State.Tasks = await _taskRepository.ListAsync();
		State.HasCompletedTasks = State.Tasks.Any(t => t.IsCompleted);
        Invalidate();
    }

    async Task CheckCompletedTasks()
    {
		// await _taskRepository.SaveItemAsync(task);
		State.Tasks = await _taskRepository.ListAsync();
		State.HasCompletedTasks = State.Tasks.Any(t => t.IsCompleted);
        Invalidate();
    }

    async Task CleanUpTasksAsync()
    {
        var completedTasks = State.Tasks.Where(t => t.IsCompleted).ToList();
		foreach (var task in completedTasks)
		{
			await _taskRepository.DeleteItemAsync(task);
		}

		State.Tasks = await _taskRepository.ListAsync();
		State.HasCompletedTasks = false;
		Invalidate();

    }

    async Task OnAppearingAsync()
    {
        if (!State.IsDataLoaded)
        {
            bool isSeeded = Preferences.Default.ContainsKey("is_seeded");

            if (!isSeeded)
            {
                await _seedDataService.LoadSeedDataAsync();
            }

            Preferences.Default.Set("is_seeded", true);

            await Refresh();
			State.IsDataLoaded = true;
        }
        // This means we are being navigated to
        else if (!State.IsNavigatedTo)
        {
            await Refresh();
        }
    }

    async Task LoadData()
	{
		try
		{
			SetState(s => s.IsBusy = true);

			var chartData = new List<CategoryChartData>();
			var chartColors = new List<Brush>();

			var categories = await _categoryRepository.ListAsync();
			foreach (var category in categories)
			{
				chartColors.Add(category.ColorBrush);

				var ps = State.Projects.Where(p => p.CategoryID == category.ID).ToList();
				int tasksCount = ps.SelectMany(p => p.Tasks).Count();

				chartData.Add(new(category.Title, tasksCount));
			}

			State.Projects = await _projectRepository.ListAsync();
			State.TodoCategoryColors = chartColors;
			State.TodoCategoryData = chartData;
			State.Tasks = await _taskRepository.ListAsync();
		}
		finally
		{
			SetState(s => s.IsBusy = false);
		}
	}

    private async Task Refresh()
	{
		try
        {
            SetState(s => s.IsRefreshing = true);
            await LoadData();
        }
        catch (Exception e)
        {
            _errorHandler.HandleError(e);
        }
        finally
        {
            SetState(s => s.IsRefreshing = false);
        }
	}	
}