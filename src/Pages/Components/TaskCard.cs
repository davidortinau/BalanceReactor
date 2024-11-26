using System.Linq;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using Balance.Models;
using Microsoft.Maui.Devices;
using Microsoft.Extensions.DependencyInjection;

namespace Balance.Components;

public class TaskCardState
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public partial class TaskCard : Component<TaskCardState>
{
    private Func<Task>? _action;

    [Inject]
    ILogger<TaskCard> _logger;

    [Inject]
    TaskRepository _taskRepository;

    private ProjectTask _task;

    public TaskCard(ProjectTask task)
    {
        _task = task;
    }

    public Component OnCheckChanged(Func<Task> action)
    {
        _action = action;
        return this;
    }

    protected override void OnMounted()
    {
        State.Title = _task.Title;
        State.IsCompleted = _task.IsCompleted;
        base.OnMounted();
    }

    public override VisualNode Render()
    => Border(
        Grid("*","Auto,*",
            CheckBox()
                .IsChecked(State.IsCompleted)
                .OnCheckedChanged(HandleCheckChanged),
            Label(State.Title)
                .GridColumn(1)
                .VerticalOptions(LayoutOptions.Center)
                .LineBreakMode(LineBreakMode.TailTruncation)
        )   
        .ColumnSpacing(15)
    )
    .OnTapped(() => NavigateToTaskDetails(_task))
    .StrokeShape(new RoundRectangle().CornerRadius(20))
    .Background(Theme.IsLightTheme ? ApplicationTheme.LightSecondaryBackground : ApplicationTheme.DarkSecondaryBackground);

    private async void HandleCheckChanged(object s, CheckedChangedEventArgs e)
    {
        _task.IsCompleted = e.Value;
        SetState(s => s.IsCompleted = e.Value);
        await Services.GetRequiredService<TaskRepository>().SaveItemAsync(_task);
        if (_action != null)
        {
            await _action.Invoke();
        }
    }

    private async void NavigateToTaskDetails(ProjectTask task)
    {
        try{
			await Microsoft.Maui.Controls.Shell.Current.GoToAsync<TaskDetailsProps>(
			nameof(TaskDetailsPage),
			props =>
				{
					props.Task = task;
                    props.IsExistingProject = true;
				}
			);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error navigating to task details");
		}
    }
}