using System;
using Microsoft.Extensions.DependencyInjection;
using Nitefox.App.Services.Interfaces;
using Nitefox.Client.Photino.Services;
using Nitefox.Client.Shared;
using Photino.Blazor;

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
                .SetContextMenuEnabled(false)
                .SetIconFile("nitefox_icon.ico")
                .SetTitle("Nitefox");
            
            app.MainWindow.Centered = true;
            app.MainWindow.GrantBrowserPermissions = true;

            App = app;

            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
            };

            app.Run();
        }

        public static PhotinoBlazorApp App { get; private set; }
}
}