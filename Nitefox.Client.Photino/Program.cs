using System;
using Microsoft.Extensions.DependencyInjection;
using Nitefox.App.Services.Interfaces;
using Nitefox.Client.Photino.Services;
using Nitefox.Client.Shared;
using Photino.Blazor;
using Photino.NET;

namespace Nitefox.Client.Photino
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

            appBuilder.Services
                .AddNitefoxServices()
                .AddSingleton<IFileService, PhotinoFileService>();

            // register root component and selector
            appBuilder.RootComponents.Add<NitefoxApp>("#app");

            var app = appBuilder.Build();

            // customize window
            app.MainWindow
                .SetMinWidth(650)
                .SetMinHeight(700)
                .SetContextMenuEnabled(false)
                .SetIconFile("nitefox_icon.ico")
                .SetTitle(""); // To avoid creating a start menu shortcut
            
            app.MainWindow.Centered = true;
            app.MainWindow.GrantBrowserPermissions = true;
            app.MainWindow.WindowCreated += (sender, _) =>
            {
                // Set title after window is created to avoid errors
                if (sender is not PhotinoWindow window) return;
                window.Title = "Nitefox";
            };

            App = app;

            AppDomain.CurrentDomain.UnhandledException += (_, error) =>
            {
                app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
            };

            app.Run();
        }

        public static PhotinoBlazorApp App { get; private set; }
}
}