using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RSTechTestApplication.Domain.Contracts;
using RSTechTestApplication.Domain.Entities;
using RSTechTestApplication.Presentation.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RSTechTestApplication.Presentation.ViewModels
{
    public partial class TaskItemViewModel : ObservableValidator
    {
        private readonly ITaskRepository _taskRepository;

        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "The title is required")]
        [MinLength(1, ErrorMessage = "Minimum 1 character")]
        [MaxLength(100, ErrorMessage = "Maximum 100 characters")]
        private string title;

        public bool IsInputValid => Title != null && (Title.Length >= 1 & Title.Length <= 100);

        public string? TitleError => GetErrors(nameof(Title))
            .OfType<ValidationResult>()
            .FirstOrDefault()?.ErrorMessage;

        [ObservableProperty]
        private bool isCompleted;

        public TaskItemViewModel(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public TaskItemViewModel(TaskEntity model, ITaskRepository taskRepository)
        {
            Id = model.Id;
            title = model.Title;
            isCompleted = model.IsCompleted;
            CreatedAt = model.CreatedAt.ToLocalTime();
            _taskRepository = taskRepository;
        }

        partial void OnIsCompletedChanged(bool value)
        {
            _taskRepository.UpdateCompletionStatusAsync(Id, value).SafeFireAndForget();
        }

        public void SetLocalTime()
        {
            CreatedAt = CreatedAt.ToLocalTime();
        }

        [RelayCommand]
        private void AddTask()
        {
            if (IsInputValid)
            {
                Id = Guid.NewGuid();
                CreatedAt = DateTime.Now;
                CloseRequested?.Invoke(true);
            }
        }

        public event Action<bool>? CloseRequested;

        [RelayCommand]
        public void Close()
        {
            CloseRequested?.Invoke(false);
        }

        public TaskEntity ToEntity()
        {
            return new TaskEntity(Id, Title, IsCompleted, CreatedAt.ToUniversalTime());
        }
    }
}
