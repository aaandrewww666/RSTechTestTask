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

            services.AddSingleton<IDialogService, DialogService>();

            services.AddTransient<AddEditTaskWindow>();

            services.AddSingleton<MainViewModel>();

            services.AddLogging(builder => builder.AddDebug());
            return services;
        }
    }
}
