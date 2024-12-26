using Grpc.Core;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.GrpcHost.Jobs;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.GrpcHost.Services
{
    public class JobService : Jobs.JobService.JobServiceBase
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public override async Task<JobResponse> GetJob(JobRequest request, ServerCallContext context)
        {
            var job = await _jobRepository.GetJobByIdAsync(request.JobId);
            if (job == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Job not found"));
            }

            return MapJobToResponse(job);
        }

        public override async Task<JobsResponse> GetAllJobs(EmptyRequest request, ServerCallContext context)
        {
            var jobs = await _jobRepository.GetAllJobsAsync();

            var response = new JobsResponse();
            response.Jobs.AddRange(jobs.Select(MapJobToResponse));
            return response;
        }

        public override async Task<JobsResponse> GetJobsByUser(JobsByUserRequest request, ServerCallContext context)
        {
            var jobs = await _jobRepository.GetJobsByUserAsync(request.ChatId);

            var response = new JobsResponse();
            response.Jobs.AddRange(jobs.Select(MapJobToResponse));
            return response;
        }

        public override async Task<ResultMessage> AddJob(AddJobRequest request, ServerCallContext context)
        {
            var newJob = new Job
            {
                Title = request.Title,
                URL = request.Url,
                FileType = request.FileType,
                Sourse = request.Source,
                ResultStatus = request.ResultStatus,
                ChatId = request.ChatId
            };

            await _jobRepository.AddJobAsync(newJob);

            return new ResultMessage
            {
                Message = "Job successfully added."
            };
        }

        public override async Task<ResultMessage> UpdateJob(UpdateJobRequest request, ServerCallContext context)
        {
            var job = await _jobRepository.GetJobByIdAsync(request.JobId);
            if (job == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Job not found"));
            }

            job.Title = request.Title;
            job.URL = request.Url;
            job.FileType = request.FileType;
            job.Sourse = request.Source;
            job.ResultStatus = request.ResultStatus;

            await _jobRepository.UpdateJobAsync(job);

            return new ResultMessage
            {
                Message = "Job successfully updated."
            };
        }

        public override async Task<ResultMessage> DeleteJob(JobRequest request, ServerCallContext context)
        {
            var job = await _jobRepository.GetJobByIdAsync(request.JobId);
            if (job == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Job not found"));
            }

            await _jobRepository.DeleteJobAsync(job.JobId);

            return new ResultMessage
            {
                Message = "Job successfully deleted."
            };
        }

        private JobResponse MapJobToResponse(Job job)
        {
            return new JobResponse
            {
                JobId = job.JobId,
                Title = job.Title,
                Url = job.URL,
                FileType = job.FileType,
                Source = job.Sourse,
                ResultStatus = job.ResultStatus
            };
        }
    }  
}
