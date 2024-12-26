using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Business.Repositories
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<Job?> GetJobByIdAsync(int jobId);
        Task<IEnumerable<Job>> GetJobsByUserAsync(long chatId);
        Task AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(int jobId);
    }
}
