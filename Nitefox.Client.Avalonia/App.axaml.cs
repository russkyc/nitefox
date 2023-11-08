using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaBlazorWebView;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Nitefox.App.Services.Interfaces;
using Nitefox.Client.Avalonia.Services;
using Nitefox.Client.Shared;

namespace Nitefox.Client.Avalonia;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new NitefoxWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    public override void RegisterServices()
    {
        base.RegisterServices();

        // if you use BlazorWebView, please setting for blazor 
        AvaloniaBlazorWebViewBuilder.Initialize(default, setting =>
        {
            //this is setting for blazor 
            setting.ComponentType = typeof(NitefoxApp);
            setting.Selector = "#app";

            setting.ResourceAssembly = typeof(NitefoxApp).Assembly;
        }, inject =>
        {
            inject.AddBlazorWebView()
                .AddMudServices()
                .AddNitefoxServices()
                .AddSingleton<IFileService, AvaloniaFileService>();
        });
    }
}