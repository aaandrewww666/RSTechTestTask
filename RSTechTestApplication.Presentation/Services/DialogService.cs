using Avalonia.Controls;
using RSTechTestApplication.Presentation.ViewModels;
using RSTechTestApplication.Presentation.Views;
using System;
using System.Threading.Tasks;

namespace RSTechTestApplication.Presentation.Services
{
    public class DialogService : IDialogService
    {
        private readonly Window _window;

        public DialogService(Window window)
        {
            _window = window;
        }

        public async Task<bool> ShowDialogAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class
        {
            var dialog = CreateDialog(viewModel);
            dialog.DataContext = viewModel;
            return await dialog.ShowDialog<bool>(_window);
        }

        private static Window CreateDialog<TViewModel>(TViewModel viewModel)
        {
            return viewModel switch
            {
                TaskItemViewModel => new AddEditTaskWindow(),
                _ => throw new ArgumentException($"No dialog for {typeof(TViewModel).Name}")
            };
        }
    }
}
