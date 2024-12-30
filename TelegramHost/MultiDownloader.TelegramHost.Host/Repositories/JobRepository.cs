using MultiDownloader.DatabaseApi.GrpcHost.Jobs;
using MultiDownloader.TelegramHost.Models;
using MultiDownloader.TelegramHost.Models.Enums;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;

namespace MultiDownloader.TelegramHost.Host.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly JobService.JobServiceClient _jobServiceClient;

        public JobRepository(JobService.JobServiceClient jobServiceClient)
        {
            _jobServiceClient = jobServiceClient;
        }

        public async Task<Job> GetJobAsync(int jobId)
        {
            var request = new JobRequest { JobId = jobId };
            var response = await _jobServiceClient.GetJobAsync(request);

            return MapToLocalJob(response);
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            var request = new EmptyRequest();
            var response = await _jobServiceClient.GetAllJobsAsync(request);

            return response.Jobs.Select(MapToLocalJob);
        }

        public async Task<IEnumerable<Job>> GetJobsByUserAsync(long chatId)
        {
            var request = new JobsByUserRequest { ChatId = chatId };
            var response = await _jobServiceClient.GetJobsByUserAsync(request);

            return response.Jobs.Select(MapToLocalJob);
        }

        public async Task<string> AddJobAsync(AddJobRequest job)
        {
            var response = await _jobServiceClient.AddJobAsync(job);
            return response.Message;
        }

        public async Task<string> UpdateJobAsync(UpdateJobRequest job)
        {
            var response = await _jobServiceClient.UpdateJobAsync(job);
            return response.Message;
        }

        public async Task<string> DeleteJobAsync(int jobId)
        {
            var request = new JobRequest { JobId = jobId };
            var response = await _jobServiceClient.DeleteJobAsync(request);
            return response.Message;
        }

        private Job MapToLocalJob(JobResponse response)
        {
            return new Job
            {
                JobId = response.JobId,
                Sourse = Enum.TryParse<JobSourse>(response.Source, true, out var source) ? source : JobSourse.Unknown,
                FileType = Enum.TryParse<FileType>(response.FileType, true, out var fileType) ? fileType : FileType.Unknown,
                URL = response.Url,
                Title = response.Title,
                ResultStatus = Enum.TryParse<ResultStatus>(response.ResultStatus, true, out var resultStatus) ? resultStatus : ResultStatus.InProgress,
            };
        }

        public Task<string> AddJobAsync(Job job)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateJobAsync(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
