using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace RSTechTestApplication.Presentation.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
        {
            e.Cancel = true; // because we either close manually or cancel closing

            Closing -= Window_OnClosing;
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
            {
                desktopApp.Shutdown();
            }
        }
    }
}