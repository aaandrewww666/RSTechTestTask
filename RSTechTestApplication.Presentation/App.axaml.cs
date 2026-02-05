using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RSTechTestApplication.Infrastructure.Database.Initializing;
using RSTechTestApplication.Presentation.ViewModels;
using RSTechTestApplication.Presentation.Views;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                var splashWindow = new SplashWindow();
                desktop.MainWindow = splashWindow;
                splashWindow.Show();

                splashWindow.Loaded += async (_, _) =>
                {
                    var databaseInitService = _services.GetRequiredService<DatabaseInitializer>();
                    var initMigrationsResult = await databaseInitService.InitializeMigrationsWithRetryAsync();

                    _logger.LogInformation(
                        "Database initialization completed. Success: {IsSuccess}, MigrationsApplied: {MigrationsApplied}, ErrorMessage: {ErrorMessage}",
                        initMigrationsResult.IsSuccess,
                        initMigrationsResult.MigrationsApplied,
                        initMigrationsResult.ErrorMessage);

                    if (!initMigrationsResult.IsSuccess)
                    {
                        await ShowDatabaseError(initMigrationsResult, splashWindow);
                    }

                    MainWindow mainWindow = _services.GetRequiredService<MainWindow>();

                    MainViewModel mainViewModel = _services.GetRequiredService<MainViewModel>();

                    mainWindow.DataContext = mainViewModel;

                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                    splashWindow.Close();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private async Task ShowDatabaseError(
            DatabaseInitResult result,
            Window window)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "RSTechTestApplication: database error",
                $"{result.ErrorMessage}",
                ButtonEnum.Ok,
                Icon.Error).ShowWindowDialogAsync(window);
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