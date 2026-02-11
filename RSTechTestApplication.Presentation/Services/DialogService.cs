using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using RSTechTestApplication.Presentation.Services.Contracts;
using RSTechTestApplication.Presentation.ViewModels;
using RSTechTestApplication.Presentation.Views;
using System;
using System.Threading.Tasks;

namespace RSTechTestApplication.Presentation.Services
{
    public sealed class DialogService : IDialogService
    {
        private readonly MainWindow _window;
        private readonly IServiceProvider _serviceProvider;

        public DialogService(MainWindow window, IServiceProvider serviceProvider)
        {
            _window = window;
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> ShowDialogAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class
        {
            var dialog = CreateDialog(viewModel);
            dialog.DataContext = viewModel;
            return await dialog.ShowDialog<bool>(_window);
        }

        private Window CreateDialog<TViewModel>(TViewModel viewModel)
        {
            return viewModel switch
            {
                TaskItemViewModel => _serviceProvider.GetRequiredService<AddEditTaskWindow>(),
                _ => throw new ArgumentException($"No dialog for {typeof(TViewModel).Name}")
            };
        }
    }
}
