using System;
using System.Threading.Tasks;

namespace RSTechTestApplication.Presentation.Extensions
{
    public static class TaskExtensions
    {
        public static void SafeFireAndForget(
            this Task task,
            Action<Exception>? onFailure = null)
        {
            _ = ExecuteAsync(task, onFailure);
        }

        /// <summary>
        /// Executes a callback function when a Task encounters an exception.
        /// </summary>
        private static async Task ExecuteAsync(Task task, Action<Exception>? onFailure)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                onFailure?.Invoke(ex);
            }
        }
    }
}
