using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public class JobStore
    {
        public ConcurrentQueue<SortJob> Jobs = new ConcurrentQueue<SortJob>();
        public ConcurrentDictionary<string, SortJob> JobDictionary = new ConcurrentDictionary<string, SortJob>();

        public async Task AddJob(SortJob job)
        {
            await Task.Run(() =>
            {
                Jobs.Enqueue(job);
                JobDictionary.TryAdd(job.Id.ToString(), job);
            });
        }

        public SortJob[] GetJobs() => JobDictionary.Values.ToArray();
        public SortJob GetJob(string jobId) => JobDictionary[jobId];
        
    }
}
