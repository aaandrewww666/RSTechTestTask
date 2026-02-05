using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Domain.Contracts;
using RSTechTestApplication.Domain.Entities;
using RSTechTestApplication.Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RSTechTestApplication.Presentation.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly IServiceProvider _services;
        private readonly ITaskRepository _taskRepository;

        public ObservableCollection<TaskItemViewModel> Tasks { get; } = [];

        [ObservableProperty]
        public TaskItemViewModel? selectedTask;

        [ObservableProperty]
        private bool isLoading;

        public MainViewModel(IServiceProvider services, ILogger<MainViewModel> logger, ITaskRepository taskRepository)
        {
            _services = services;
            _logger = logger;
            _taskRepository = taskRepository;

            LoadTasksAsync().SafeFireAndForget(OnLoadingException);
        }

        public void OnLoadingException(Exception ex)
        {
            _logger.LogError($"Exception occured on loading tasks: {ex.Message}");
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
                IEnumerable<TaskEntity> tasks = [new TaskEntity("oao"), new TaskEntity("oao2")];

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
        /// </summary>
        [RelayCommand]
        public async Task AddNewTaskAsync()
        {
            var vm = new TaskItemViewModel(_taskRepository);

        }

        /// <summary>
        /// A command to delete task.
        /// </summary>
        [RelayCommand]
        public async Task DeleteTaskAsync()
        {
            if (SelectedTask == null)
            {
                return;
            }
            Tasks.Remove(SelectedTask);
            SelectedTask = null;

        }
    }
}
