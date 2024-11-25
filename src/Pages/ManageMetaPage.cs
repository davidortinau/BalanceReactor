using System.Linq;
using Balance.Components;
using Balance.Controls;
using Balance.Resources.Styles;
using CommunityTemplate.Pages.Controls;
using MauiReactor;
using MauiReactor.Shapes;
using Microsoft.Maui.Devices;

namespace Balance.Pages;

class ManageMetaState
{
    public List<Category> Categories { get; set; }
    public List<Tag> Tags { get; set; }
}

partial class ManageMetaPage : Component<ManageMetaState>
{
    [Inject]
    CategoryRepository _categoryRepository;

    [Inject]
    TagRepository _tagRepository;
    
    [Inject]
    SeedDataService _seedDataService;

    protected override async void OnMounted()
    {
        State.Categories = await _categoryRepository.ListAsync();
        State.Tags = await _tagRepository.ListAsync();

        base.OnMounted();
    }

    public override VisualNode Render()
        => ContentPage(
            ToolbarItem("Reset App").OnClicked(ResetAppAsync),
            ScrollView(
                VStack(
                    Label("Categories").ThemeKey("Title2"),
                    VStack(
                        State.Categories.Select(c => RenderCategory(c)).ToArray()
                    ).Spacing(ApplicationTheme.LayoutSpacing),

                    Grid("Auto","*,Auto",
                        Button("Save").HeightRequest(ApplicationTheme.ButtonMinimumSize).OnClicked(SaveCategoriesAsync),
                        Button().ImageSource(ApplicationTheme.IconAdd).Style("SquareButton").OnClicked(AddCategoryAsync).GridColumn(1)
                    ).ColumnSpacing(ApplicationTheme.LayoutSpacing).Margin(0, 10),

                    Label("Tags").ThemeKey("Title2"),
                    VStack(
                        State.Tags.Select(t => RenderTag(t)).ToArray()
                    ).Spacing(ApplicationTheme.LayoutSpacing),

                    Grid("Auto", "*,Auto",
                        Button("Save").HeightRequest(ApplicationTheme.ButtonMinimumSize).OnClicked(() => SaveTagsAsync()),
                        Button().ImageSource(ApplicationTheme.IconAdd).Style("SquareButton").OnClicked(() => AddTagAsync()).GridColumn(1)
                    ).ColumnSpacing(ApplicationTheme.LayoutSpacing).Margin(0, 10)
                ).Spacing(ApplicationTheme.LayoutSpacing).Padding(ApplicationTheme.LayoutPadding)
            )
        );

    private VisualNode RenderCategory(Category category)
    {
        return Grid("Auto","4*,3*,30,Auto",
            Entry().Text(category.Title).GridColumn(0).OnTextChanged((s) => category.Title = s),
            Entry().Text(category.Color).GridColumn(1).OnTextChanged((s) => category.Color = s),
            // .Behaviors(new TextValidationBehavior
            // {
            //     InvalidStyle = "InvalidEntryStyle",
            //     Flags = ValidationFlags.ValidateOnUnfocusing,
            //     RegexPattern = "^#(?:[0-9a-fA-F]{3}){1,2}$"
            // }),
            BoxView().HeightRequest(30).WidthRequest(30).VerticalOptions(LayoutOptions.Center).Color(Color.FromArgb(category.Color)).GridColumn(2),
            Button().ImageSource(ApplicationTheme.IconDelete).BackgroundColor(Colors.Transparent).OnClicked(async () => await DeleteCategoryAsync(category)).GridColumn(3)
        ).ColumnSpacing(ApplicationTheme.LayoutSpacing);
    }

    private VisualNode RenderTag(Tag tag)
    {
        return Grid("Auto","4*,3*,30,Auto",
            Entry().Text(tag.Title).GridColumn(0).OnTextChanged((s) => tag.Title = s),
            Entry().Text(tag.Color).GridColumn(1).OnTextChanged((s) => tag.Color = s),
            // .Behaviors(new TextValidationBehavior
            // {
            //     InvalidStyle = "InvalidEntryStyle",
            //     Flags = ValidationFlags.ValidateOnUnfocusing,
            //     RegexPattern = "^#(?:[0-9a-fA-F]{3}){1,2}$"
            // }),
            BoxView().HeightRequest(30).WidthRequest(30).VerticalOptions(LayoutOptions.Center).Color(Color.FromArgb(tag.Color)).GridColumn(2),
            Button().ImageSource(ApplicationTheme.IconDelete).BackgroundColor(Colors.Transparent).OnClicked(() => DeleteTagAsync(tag)).GridColumn(3)
        ).ColumnSpacing(ApplicationTheme.LayoutSpacing);
    }

    async Task ResetAppAsync()
    {
        Preferences.Default.Remove("is_seeded");
        await _seedDataService.LoadSeedDataAsync();
        Preferences.Default.Set("is_seeded", true);
        await Microsoft.Maui.Controls.Shell.Current.GoToAsync("//main");
    }

    async Task SaveCategoriesAsync()
    {
        foreach (var category in State.Categories)
        {
            await _categoryRepository.SaveItemAsync(category);
        }

        await AppShell.DisplayToastAsync("Categories saved");
    }

    async Task AddCategoryAsync()
    {
        var category = new Category();
        SetState(s => s.Categories.Add(category));
        await _categoryRepository.SaveItemAsync(category);
		await AppShell.DisplayToastAsync("Category added");
    }

    async Task DeleteCategoryAsync(Category category)
    {
        SetState(s => s.Categories.Remove(category));
		await _categoryRepository.DeleteItemAsync(category);
		await AppShell.DisplayToastAsync("Category deleted");
    }

    async Task SaveTagsAsync()
    {
        foreach (var tag in State.Tags)
        {
            await _tagRepository.SaveItemAsync(tag);
        }

        await AppShell.DisplayToastAsync("Tags saved");
    }

    private async Task AddTagAsync()
    {
        var tag = new Tag();
        SetState(s => s.Tags.Add(tag));
        await _tagRepository.SaveItemAsync(tag);
        await AppShell.DisplayToastAsync("Tag added");
    }

    async Task DeleteTagAsync(Tag tag)
    {
        SetState(s => s.Tags.Remove(tag));
        await _tagRepository.DeleteItemAsync(tag);
        await AppShell.DisplayToastAsync("Tag deleted");
    }
}