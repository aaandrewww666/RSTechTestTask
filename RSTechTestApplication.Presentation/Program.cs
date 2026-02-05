using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Infrastructure;
using System;

namespace RSTechTestApplication.Presentation
{
    internal sealed class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            BuildAvaloniaApp(serviceProvider)
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp(IServiceProvider services)
            => AppBuilder.Configure(() => new App(services))
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
            });

            services
                .AddPresentation()
                .AddInfrastructure();

            return services.BuildServiceProvider();
        }
    }
}
