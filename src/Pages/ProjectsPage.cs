using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balance.Data;
using Balance.Models;
using Balance.Resources.Styles;
using MauiReactor;
using Microsoft.Extensions.Logging;

namespace Balance.Pages
{
    public class ProjectsPageState
    {
        public bool IsBusy { get; set; }
        public List<Project> Projects { get; set; } = new();
    }

    public partial class ProjectsPage : Component<ProjectsPageState>
    {
        [Inject]
        ProjectRepository _projectRepository;

        [Inject]
        ILogger<ProjectsPage> _logger;

        protected override async void OnMounted()
        {
            base.OnMounted();
            await LoadProjects();
        }

        private async Task LoadProjects()
        {
            try
            {
                State.IsBusy = true;
                State.Projects = await _projectRepository.ListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects");
            }
            finally
            {
                State.IsBusy = false;
            }
        }

        public override VisualNode Render()
            => ContentPage(
                Grid(
                    VStack(
                        State.Projects.Select(project => 
                            Border(
                                VStack
                                (
                                    Label(project.Name).FontSize(24),
                                    Label(project.Description)
                                )
                                .Padding(10)
                            ).OnTapped(() => NavigateToProject(project))
                        ).ToArray()
                    )
                    .Spacing(ApplicationTheme.LayoutSpacing)
                    .Padding(ApplicationTheme.LayoutPadding)
                    // new AddButton
                    // {
                    //     Command = new Command(AddProject)
                    // }
                )
            );

        private async void NavigateToProject(Project project)
        {
            try{
                await Microsoft.Maui.Controls.Shell.Current.GoToAsync<ProjectDetailProps>(
                nameof(ProjectDetailsPage),
                props =>
                    {
                        props.Project = project;
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to project details");
            }
        }

        private void AddProject()
        {
            // Implement add project logic here
        }
    }
}