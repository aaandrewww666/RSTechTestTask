using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Presentation.ViewModels;
using RSTechTestApplication.Presentation.Views;
using System;
using System.Linq;

namespace RSTechTestApplication.Presentation
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<App> _logger;

        public App(IServiceProvider services)
        {
            _services = services;
            _logger = _services.GetRequiredService<ILogger<App>>();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                MainWindow mainWindow = _services.GetRequiredService<MainWindow>();

                MainViewModel mainViewModel = _services.GetRequiredService<MainViewModel>();

                mainWindow.DataContext = mainViewModel;

                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}