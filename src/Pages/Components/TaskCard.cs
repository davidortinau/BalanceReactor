using System.Linq;
using Balance.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using Balance.Models;
using Microsoft.Maui.Devices;

namespace Balance.Components;

public class TaskCardState
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public partial class TaskCard : Component<TaskCardState>
{
    private ProjectTask _task;

    public TaskCard(ProjectTask task)
    {
        _task = task;
    }

    protected override void OnMounted()
    {
        State.Title = _task.Title;
        State.IsCompleted = _task.IsCompleted;
        base.OnMounted();
    }

    public override VisualNode Render()
    => Border(
        HStack(
            CheckBox()
                .IsChecked(State.IsCompleted)
                .OnCheckedChanged((s, e) => SetState(s => s.IsCompleted = e.Value)),
            Label(State.Title)
                .VerticalOptions(LayoutOptions.Center)
                .LineBreakMode(LineBreakMode.TailTruncation)
        )
        .Spacing(15)        
        .Padding(DeviceInfo.Platform == DevicePlatform.WinUI ? 20 : 15)
    )
    .StrokeShape(new RoundRectangle().CornerRadius(20))
    .Background(Theme.IsLightTheme ? ApplicationTheme.LightSecondaryBackground : ApplicationTheme.DarkSecondaryBackground);
}