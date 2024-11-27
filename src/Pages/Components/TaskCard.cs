using System.Linq;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using Balance.Models;
using Microsoft.Maui.Devices;
using Microsoft.Extensions.DependencyInjection;

namespace Balance.Components;

public partial class TaskCard : Component
{
    [Prop]
    ProjectTask _task;

    [Prop]
    Func<ProjectTask, bool, Task>? _onCompletedChanged;

    [Inject]
    ILogger<TaskCard> _logger;

    public override VisualNode Render()
    => Border(
        Grid("*","Auto,*",
            CheckBox()
                .IsChecked(_task.IsCompleted)
                .OnCheckedChanged(HandleCheckChanged),
            Label(_task.Title)
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
        if (_onCompletedChanged != null)
        {
            await _onCompletedChanged(_task, e.Value);
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