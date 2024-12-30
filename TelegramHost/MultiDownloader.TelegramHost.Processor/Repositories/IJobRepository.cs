using MultiDownloader.TelegramHost.Models;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Repositories
{
    public interface IJobRepository
    {
        Task<Job> GetJobAsync(int jobId);
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<IEnumerable<Job>> GetJobsByUserAsync(long chatId);
        Task<string> AddJobAsync(Job job);
        Task<string> UpdateJobAsync(Job job);
        Task<string> DeleteJobAsync(int jobId);
    }
}
