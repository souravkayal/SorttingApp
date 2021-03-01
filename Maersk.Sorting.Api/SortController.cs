using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    public class SortController : ControllerBase
    {
        private readonly ISortJobProcessor _sortJobProcessor;

        public SortController(ISortJobProcessor sortJobProcessor)
        {
            _sortJobProcessor = sortJobProcessor;
        }

        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }


        [HttpPost]
        public async Task<ActionResult<SortJob>> EnqueueJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            await _sortJobProcessor.ProcessAsync(pendingJob);

            return Ok(pendingJob);
        }


        [HttpGet]
        public ActionResult<SortJob[]> GetJobs()
        {
            return _sortJobProcessor.GetJobs();
        }


        [HttpGet("{jobId}")]
        public ActionResult<SortJob> GetJob(Guid jobId)
        {
            return _sortJobProcessor.GetJob(jobId.ToString());
        }
    }
}
