using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor;

using Balance.Pages;
using System.Diagnostics;

namespace Balance;

class AppShell : Component
{
    public AppShell()
    {
        MauiExceptions.UnhandledException += (sender, args) =>
        {
            Debug.WriteLine(args.ExceptionObject);
            throw (Exception)args.ExceptionObject;
        };
    }
    public override VisualNode Render()
        => Shell(
            FlyoutItem("MainPage",
                ShellContent()
                    .Title("MainPage")
                    .RenderContent(()=>new MainPage())
            ),
            FlyoutItem("OtherPage",
                ShellContent()
                    .Title("OtherPage")
                    .RenderContent(()=>new OtherPage())
            )
        );
}

