using System.Linq;
using Balance.Components;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;

namespace Balance.Pages;

public class MainPageState
{
    public bool IsBusy { get; set; }

    public bool IsRefreshing { get; set; }

    public List<CategoryChartData> TodoCategoryData {get;set;}

	public List<Brush> TodoCategoryColors {get;set;}

	public List<ProjectTask> Tasks {get;set;}

	public List<Project> Projects {get;set;}
    
}

public partial class MainPage : Component<MainPageState>
{
    private bool _dataLoaded;

    private bool _isNavigatedTo;

    [Inject]
    ProjectRepository _projectRepository;

    [Inject]
    CategoryRepository _categoryRepository;

    [Inject]
    TaskRepository _taskRepository;

    [Inject]
    SeedDataService _seedDataService;

    protected override async void OnMounted()
    {
        base.OnMounted();

        if (!_dataLoaded)
		{
			await InitData(_seedDataService);
			_dataLoaded = true;
			await Refresh();
		}
		// This means we are being navigated to
		else if (!_isNavigatedTo)
		{
			await Refresh();
		}
    }
    public override VisualNode Render()
     => ContentPage(
            Grid(
                VScrollView(
                    VStack(
                        Label("Projects").ThemeKey("Title2"),
                        HScrollView(
                            HStack(
								State.Projects.Select(p => new ProjectCard(p).Width(200)).ToArray()
                            )
                            .Spacing(15)
                            .Padding(30,0)
                        )
                        .Margin(-30,0)
                    )
                    .Spacing(ApplicationTheme.LayoutSpacing)
                    .Padding(ApplicationTheme.LayoutPadding)
                )
            )
        ).Title(DateTime.Now.ToString("dddd, MMM d"));

    private async Task LoadData()
	{
		try
		{
			State.IsBusy = true;

			State.Projects = await _projectRepository.ListAsync();

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

			State.TodoCategoryData = chartData;
			State.TodoCategoryColors = chartColors;

			State.Tasks = await _taskRepository.ListAsync();
		}
		finally
		{
			State.IsBusy = false;
		}
	}

	private async Task InitData(SeedDataService seedDataService)
	{
		bool isSeeded = Preferences.Default.ContainsKey("is_seeded");

		if (!isSeeded)
		{
			await seedDataService.LoadSeedDataAsync();
		}

		Preferences.Default.Set("is_seeded", true);
		await Refresh();
	}

    private async Task Refresh()
	{
		try
		{
			State.IsRefreshing = true;
			await LoadData();
		}
		catch (Exception e)
		{
			// _errorHandler.HandleError(e);
		}
		finally
		{
			State.IsRefreshing = false;
		}
	}
}