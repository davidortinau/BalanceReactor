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

    protected override async void OnMounted()
    {
        State.Categories = await _categoryRepository.ListAsync();
        State.Tags = await _tagRepository.ListAsync();

        base.OnMounted();
    }

    public override VisualNode Render()
        => ContentPage(
            ToolbarItem("Reset App").OnClicked(() => ResetApp()),
            ScrollView(
                VStack(
                    Label("Categories").ThemeKey("Title2"),
                    VStack(
                        State.Categories.Select(c => RenderCategory(c)).ToArray()
                    ).Spacing(ApplicationTheme.LayoutSpacing),

                    Grid("Auto,Auto", "*",
                        Button("Save").HeightRequest(ApplicationTheme.ButtonMinimumSize).OnClicked(() => SaveCategories()),
                        Button().ImageSource(ApplicationTheme.IconAdd).Style("SquareButton").OnClicked(() => AddCategory()).GridColumn(1)
                    ).ColumnSpacing(ApplicationTheme.LayoutSpacing).Margin(0, 10),

                    Label("Tags").ThemeKey("Title2"),
                    VStack(
                        State.Tags.Select(t => RenderTag(t)).ToArray()
                    ).Spacing(ApplicationTheme.LayoutSpacing),

                    Grid("Auto,Auto", "*",
                        Button("Save").HeightRequest(ApplicationTheme.ButtonMinimumSize).OnClicked(() => SaveTags()),
                        Button().ImageSource(ApplicationTheme.IconAdd).Style("SquareButton").OnClicked(() => AddTag()).GridColumn(1)
                    ).ColumnSpacing(ApplicationTheme.LayoutSpacing).Margin(0, 10)
                ).Spacing(ApplicationTheme.LayoutSpacing).Padding(ApplicationTheme.LayoutPadding)
            )
        );

    private VisualNode RenderCategory(Category category)
    {
        return Grid("Auto","4*,3*,30,Auto",
            Entry().Text(category.Title).GridColumn(0),
            Entry().Text(category.Color).GridColumn(1),
            // .Behaviors(new TextValidationBehavior
            // {
            //     InvalidStyle = "InvalidEntryStyle",
            //     Flags = ValidationFlags.ValidateOnUnfocusing,
            //     RegexPattern = "^#(?:[0-9a-fA-F]{3}){1,2}$"
            // }),
            BoxView().HeightRequest(30).WidthRequest(30).VerticalOptions(LayoutOptions.Center).Color(Color.FromArgb(category.Color)).GridColumn(2),
            Button().ImageSource(ApplicationTheme.IconDelete).BackgroundColor(Colors.Transparent).OnClicked(() => DeleteCategory(category)).GridColumn(3)
        ).ColumnSpacing(ApplicationTheme.LayoutSpacing);
    }

    private VisualNode RenderTag(Tag tag)
    {
        return Grid("Auto","4*,3*,30,Auto",
            Entry().Text(tag.Title).GridColumn(0),
            Entry().Text(tag.Color).GridColumn(1),
            // .Behaviors(new TextValidationBehavior
            // {
            //     InvalidStyle = "InvalidEntryStyle",
            //     Flags = ValidationFlags.ValidateOnUnfocusing,
            //     RegexPattern = "^#(?:[0-9a-fA-F]{3}){1,2}$"
            // }),
            BoxView().HeightRequest(30).WidthRequest(30).VerticalOptions(LayoutOptions.Center).Color(Color.FromArgb(tag.Color)).GridColumn(2),
            Button().ImageSource(ApplicationTheme.IconDelete).BackgroundColor(Colors.Transparent).OnClicked(() => DeleteTag(tag)).GridColumn(3)
        ).ColumnSpacing(ApplicationTheme.LayoutSpacing);
    }

    private void ResetApp()
    {
        throw new NotImplementedException();
    }

    private void SaveCategories()
    {
        throw new NotImplementedException();
    }

    private void AddCategory()
    {
        throw new NotImplementedException();
    }

    private void DeleteCategory(Category category)
    {
        throw new NotImplementedException();
    }

    private void SaveTags()
    {
        throw new NotImplementedException();
    }

    private void AddTag()
    {
        throw new NotImplementedException();
    }

    private void DeleteTag(Tag tag)
    {
        throw new NotImplementedException();
    }
}