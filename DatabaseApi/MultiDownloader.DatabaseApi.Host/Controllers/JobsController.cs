using Microsoft.AspNetCore.Mvc;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;

        public JobsController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        // GET: api/jobs
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();
            return Ok(jobs);
        }

        // GET: api/jobs/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound(new { Message = "Job not found" });
            }
            return Ok(job);
        }

        // POST: api/jobs
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] Job job)
        {
            if (job == null)
            {
                return BadRequest(new { Message = "Invalid job data" });
            }

            await _jobRepository.AddJobAsync(job);
            return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, job);
        }

        // PUT: api/jobs/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] Job job)
        {
            if (job == null || id != job.JobId)
            {
                return BadRequest(new { Message = "Invalid job data or ID mismatch" });
            }

            var existingJob = await _jobRepository.GetJobByIdAsync(id);
            if (existingJob == null)
            {
                return NotFound(new { Message = "Job not found" });
            }

            await _jobRepository.UpdateJobAsync(job);
            return NoContent();
        }

        // DELETE: api/jobs/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var existingJob = await _jobRepository.GetJobByIdAsync(id);
            if (existingJob == null)
            {
                return NotFound(new { Message = "Job not found" });
            }

            await _jobRepository.DeleteJobAsync(id);
            return NoContent();
        }
    }

}
