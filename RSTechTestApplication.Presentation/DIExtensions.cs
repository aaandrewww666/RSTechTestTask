using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Presentation.Services;
using RSTechTestApplication.Presentation.Services.Contracts;
using RSTechTestApplication.Presentation.ViewModels;
using RSTechTestApplication.Presentation.Views;

namespace RSTechTestApplication.Presentation
{
    public static class DIExtensions
    {
        /// <summary>
        /// Services for Presentation layer
        /// </summary>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AddEditTaskWindow>();

            services.AddSingleton<MainViewModel>();

            services.AddSingleton<IDialogService>(provider =>
            {
                var mainWindow = provider.GetRequiredService<MainWindow>();
                return new DialogService(mainWindow); //fabric for dialog service from mainWindow
            });

            services.AddLogging(builder => builder.AddDebug());
            return services;
        }
    }
}
