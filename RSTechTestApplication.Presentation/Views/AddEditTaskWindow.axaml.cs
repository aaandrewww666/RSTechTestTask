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
        {
            vm.CloseRequested += result => Close(result);
        }
    }
}