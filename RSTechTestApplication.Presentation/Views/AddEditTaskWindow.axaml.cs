using Avalonia.Controls;
using RSTechTestApplication.Presentation.ViewModels;
using System;

namespace RSTechTestApplication.Presentation.Views;

public partial class AddEditTaskWindow : Window
{
    public AddEditTaskWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is TaskItemViewModel vm)
            vm.CloseRequested += OnCloseRequested;
    }

    protected override void OnClosed(EventArgs e)
    {
        if (DataContext is TaskItemViewModel vm)
            vm.CloseRequested -= OnCloseRequested;

        base.OnClosed(e);
    }

    private void OnCloseRequested(bool result) => Close(result);
}