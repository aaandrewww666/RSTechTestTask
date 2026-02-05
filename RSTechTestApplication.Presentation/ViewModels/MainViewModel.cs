using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Message.Avalonia;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Domain.Contracts;
using RSTechTestApplication.Domain.Entities;
using RSTechTestApplication.Presentation.Extensions;
using RSTechTestApplication.Presentation.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RSTechTestApplication.Presentation.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly ILogger<MainViewModel> _logger;
        private readonly IServiceProvider _services;
        private readonly ITaskRepository _taskRepository;

        public ObservableCollection<TaskItemViewModel> Tasks { get; } = [];

        [ObservableProperty]
        public TaskItemViewModel? selectedTask;

        [ObservableProperty]
        private bool isLoading;

        public MainViewModel(IServiceProvider services, ILogger<MainViewModel> logger, IDialogService dialogService, ITaskRepository taskRepository)
        {
            _services = services;
            _logger = logger;
            _dialogService = dialogService;
            _taskRepository = taskRepository;

            MessageManager.Default.Duration = TimeSpan.FromSeconds(5);

            LoadTasksAsync().SafeFireAndForget(OnLoadingException);
        }

        public void OnLoadingException(Exception ex)
        {
            _logger.LogError($"Exception occured on loading tasks: {ex.Message}");
            MessageManager.Default.ShowErrorMessage("Exception occured on loading tasks");
        }

        [RelayCommand]
        private void ToggleCompleted()
        {
            if (SelectedTask != null)
            {
                SelectedTask.IsCompleted = !SelectedTask.IsCompleted;
            }
        }

        /// <summary>
        /// Load all tasks from the repository
        /// </summary>
        /// <returns></returns>
        public async Task LoadTasksAsync()
        {
            IsLoading = true;
            try
            {
                _logger.LogInformation("Trying to get all tasks");
                IEnumerable<TaskEntity> tasks = await _taskRepository.GetAllAsync();

                foreach (var task in tasks)
                {
                    Tasks.Add(new TaskItemViewModel(task, _taskRepository));
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// A command to add new task.
        /// Creates a new dialog to display the user the fields to fill in a new task. 
        /// If passes ui validation - tries to add to the DB.
        /// Regardless of the result of adding to the DB adds to the list of Tasks.
        /// </summary>
        [RelayCommand]
        public async Task AddNewTaskAsync()
        {
            var vm = new TaskItemViewModel(_taskRepository);

            if (await _dialogService.ShowDialogAsync(vm))
            {
                Tasks.Add(vm); //always do even we don't have connection to DB or have exceptions

                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (await _taskRepository.AddAsync(vm.ToEntity()) != null)
                        {
                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                MessageManager.Default.ShowSuccessMessage("Successfully added to the database.");
                            });
                        }
                        else
                            MessageManager.Default.ShowErrorMessage("Failed to add new task to the database.");
                    }
                    catch
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            MessageManager.Default.ShowErrorMessage("Failed to add new task to the database.");
                        });
                    }
                });
            }
        }

        /// <summary>
        /// A command to delete task.
        /// </summary>
        [RelayCommand]
        public async Task DeleteTaskAsync()
        {
            if (SelectedTask == null) return;

            var taskToDelete = SelectedTask;

            Tasks.Remove(taskToDelete);
            SelectedTask = null;

            _ = Task.Run(async () =>
            {
                try
                {
                    await _taskRepository.SoftDeleteAsync(taskToDelete.Id);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        MessageManager.Default.ShowSuccessMessage("Successfully deleted from the database.");
                    });
                }
                catch
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        MessageManager.Default.ShowErrorMessage("Failed to delete from database.");
                    });
                }
            });
        }
    }
}
