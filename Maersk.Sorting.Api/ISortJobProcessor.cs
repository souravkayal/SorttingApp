using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessor
    {
        Task<SortJob> Process(SortJob job);

        Task ProcessAsync(SortJob job);
        SortJob[] GetJobs();
        SortJob GetJob(string jobId);
    }
}