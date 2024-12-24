using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Database.Repositories;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Host.Models
{
    public class GraphQlQueries
    {
        [UseProjection]
        [UseFiltering()]
        public async Task<IEnumerable<User>> GetUsers([Service] IUserRepository userRepository) => 
            await userRepository.GetAllUsersAsync();

        [UseProjection]
        [UseFiltering()]
        public async Task<IEnumerable<Job>> GetJobs([Service] IJobRepository jobRepository) =>
            await jobRepository.GetAllJobsAsync();
    }
}
