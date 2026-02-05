using System.Threading.Tasks;

namespace RSTechTestApplication.Presentation.Services
{
    public interface IDialogService
    {
        Task<bool> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : class;
    }
}
