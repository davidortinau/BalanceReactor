using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MauiReactor;
using Balance.Resources.Styles;
using Microsoft.Maui.Devices;

namespace CommunityTemplate.Pages.Controls
{
    public partial class AddButton : Component
    {
        [Prop]
        bool _isTask;

        [Inject]
        ILogger<AddButton> _logger;

        public override VisualNode Render()
        {
            return Button()
                .ImageSource(ApplicationTheme.IconAdd)
                .BackgroundColor(ApplicationTheme.Primary)
                .CornerRadius(30)
                .HeightRequest(60)
                .WidthRequest(60)
                .HEnd().VEnd()
                .Padding(DeviceInfo.Platform == DevicePlatform.WinUI ? new Thickness(0, 2, 4, 0) : new Thickness(0))
                .Margin(30)
                .OnClicked(() => NavigateToAdd());
        }

        private void NavigateToAdd()
        {
            try
            {
                if (_isTask)
                {
                    Microsoft.Maui.Controls.Shell.Current.GoToAsync<TaskDetailsProps>(
                        nameof(TaskDetailsPage),
                        props =>
                        {
                            props.Task = new ProjectTask();
                            props.IsExistingProject = false;
                        });
                }
                else
                {
                    Microsoft.Maui.Controls.Shell.Current.GoToAsync<ProjectDetailProps>(
                        nameof(ProjectDetailsPage),
                        props =>
                        {
                            props.Project = new Project();
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to add page");
            }
        }
    }
}