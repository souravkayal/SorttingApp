using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Job
{
    public class SortingJobService : JobStore , IHostedService, IDisposable
    {
        private readonly ILogger<SortingJobService> _logger;
        private readonly JobStore _jobStore;

        public SortingJobService(ILogger<SortingJobService> logger , JobStore jobStore)
        {
            _logger = logger;
            _jobStore = jobStore;
        }

        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                var stopwatch = Stopwatch.StartNew();

                if (_jobStore.Jobs.Count > 0)
                {
                    _jobStore.Jobs.TryDequeue(out var job);

                    if(job != null)
                    {
                        var output = job?.Input.OrderBy(n => n).ToArray();
                        
                        var duration = stopwatch.Elapsed;

                        _jobStore.JobDictionary.TryUpdate(job?.Id.ToString() ?? String.Empty,
                        new SortJob(job?.Id ?? Guid.Empty, SortJobStatus.Completed, duration, job?.Input ?? new List<int>(), output),
                        job ?? (SortJob) new Object());

                    }
                }
                
                //Probe Task Q in every 5 second
                await Task.Delay(5000);
            }
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
