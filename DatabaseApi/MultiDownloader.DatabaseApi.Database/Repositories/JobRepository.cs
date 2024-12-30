using Microsoft.EntityFrameworkCore;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Database.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly MultiDownloaderContext _context;

        public JobRepository(MultiDownloaderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync() =>
            await _context
            .Jobs
            .Include(job => job.User)
            .ToListAsync();

        public async Task<Job?> GetJobByIdAsync(int jobId) =>
            await _context
            .Jobs
            .Include(job => job.User)
            .FirstOrDefaultAsync(j => j.JobId == jobId);

        public async Task<IEnumerable<Job>> GetJobsByUserAsync(long chatId) =>
            await _context.Jobs
                .Where(job => job.ChatId == chatId)
                .ToListAsync();

        public async Task AddJobAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobAsync(int jobId)
        {
            var job = await GetJobByIdAsync(jobId);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }
    }
}
