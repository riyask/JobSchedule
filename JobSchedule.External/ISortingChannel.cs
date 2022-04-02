using JobSchedule.Shared.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobSchedule.External
{
    public interface ISortingChannel
    {
        public Task<bool> AddItemAsync(JobItem jobItem, CancellationToken ct = default);

        public IAsyncEnumerable<JobItem> ReadAllAsync(CancellationToken ct = default);

        public IEnumerable<JobItem> GetAllJob();
        public JobItem GetJobById(string id);


        void CompleteWriter(Exception ex = null);
        bool TryCompleteWriter(Exception ex = null);
    }
}
