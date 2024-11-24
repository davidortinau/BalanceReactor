## Mistakes encountered

### Error: No Shell flyout icons appearing

My mistake:

```csharp
FlyoutItem("Dashboard",
    ShellContent()
        .Title("Dashboard)
        .Icon(ApplicationTheme.IconDashboard)
        .RenderContent(()=> new MainPage())
        .Route("main")
),
```

The `.Icon` doesn't propogate up to the `FlyoutItem` from `ShellContent`.

Corrected:

```csharp
FlyoutItem("Dashboard",
    ShellContent()
        .Title("Dashboard)
        .RenderContent(()=> new MainPage())
        .Route("main")
)
.Icon(ApplicationTheme.IconDashboard),
```

### Error: doesn't render a page as root

```
Component Balance.Pages.ProjectDetailsPage doesn't render a page as root
```

Why? Whatever the Render method is returning is NOT what is expected. In my case, I was trying to figure out how to include ToolbarItems.

Wrong:

```csharp
public override VisualNode Render()
        => ContentPage(
            ScrollView(
                VStack(
                    new SfTextInputLayout{
                        Entry()
                            .Text(State.Name).OnTextChanged((s, e) => State.Name = e.NewTextValue)
                    }
                        .Hint("Name")
                        
                )
                .Spacing(ApplicationTheme.LayoutSpacing)
                .Padding(ApplicationTheme.LayoutPadding)
            ),
            ToolbarItem("Delete").OnClicked(() => DeleteProject()).IsDestructive(true)
        );
```

Correct:

```csharp
public override VisualNode Render()
        => ContentPage(
            ToolbarItem("Delete").OnClicked(() => DeleteProject()).IsDestructive(true),
            ScrollView(
                VStack(
                    new SfTextInputLayout{
                        Entry()
                            .Text(State.Name).OnTextChanged((s, e) => State.Name = e.NewTextValue)
                    }
                        .Hint("Name")
                        
                )
                .Spacing(ApplicationTheme.LayoutSpacing)
                .Padding(ApplicationTheme.LayoutPadding)
            )
            
        );
```

One of the downsides of this architecture is we don't get compile time validation for things like this.