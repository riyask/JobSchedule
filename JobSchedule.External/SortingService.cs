using JobSchedule.Data;
using JobSchedule.Shared.Model;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobSchedule.External
{
    /// <summary>
    /// class SortingService
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
    /// <seealso cref="System.IDisposable" />
    public class SortingService : Microsoft.Extensions.Hosting.BackgroundService, IDisposable
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        //To implement timer based event
        //private readonly int _refreshIntervalInSeconds = 3;

        /// <summary>
        /// The sorting channel
        /// </summary>
        private readonly ISortingChannel _sortingChannel;
        /// <summary>
        /// The application data storage
        /// </summary>
        private readonly IAppDataStorage _appDataStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortingService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="hostApplicationLifetime">The host application lifetime.</param>
        /// <param name="sortingChannel">The sorting channel.</param>
        /// <param name="appDataStorage">The application data storage.</param>
        public SortingService(ILogger logger, IHostApplicationLifetime hostApplicationLifetime, ISortingChannel sortingChannel, IAppDataStorage appDataStorage)
        {
            _logger = logger;
            _sortingChannel = sortingChannel;
            _appDataStorage = appDataStorage;
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information($"{nameof(LogEventMap.HostingService_Started) } , Host Service has started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //To Enable timer based event but we are using IAsyncEnumerable using Channel Options
                    //await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);

                    //Either read IAsyncEnumrable and process or collections and invoke parallel sorting task.
                    //Keeping iAsyncEnumerable for the sake of simplicity at the moment.                
                    var jobId = string.Empty;
                    await foreach (var jobItem in _sortingChannel.ReadAllAsync(stoppingToken))
                    {
                        jobId = jobItem.Id;
                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemReceived) } , Id = {jobId}, Message = Host Service has received this item.");

                        _appDataStorage.Add(jobItem);

                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemSortingInProgress) } , Id = {jobId}, Message = Host Service has received this item.");

                        await Task.Run(() => SortNumbers(jobItem), stoppingToken);

                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemSortingCompleted) } , Id = {jobId}, Message = Host Service has completed this item.");

                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warning(ex, "Operation cancelled occured");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception was thrown.");
                _sortingChannel.CompleteWriter(ex);
            }
            finally
            {
                _sortingChannel.TryCompleteWriter();
            }
        }

        /// <summary>
        /// Sorts the numbers.
        /// </summary>
        /// <param name="jobItem">The job item.</param>
        private void SortNumbers(JobItem jobItem)
        {
            QuickSort(jobItem.Items, 0, jobItem.Items.Count - 1);
            jobItem.UpdateJobStatus(JobStatus.Completed);
        }

        /// <summary>
        /// Quicksort.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        private void QuickSort(List<int> arr, int start, int end)
        {
            int i;
            if (start < end)
            {
                i = Partition(arr, start, end);

                QuickSort(arr, start, i - 1);
                QuickSort(arr, i + 1, end);
            }
        }

        /// <summary>
        /// Partitions the specified arr.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        private int Partition(List<int> arr, int start, int end)
        {
            int temp;
            int p = arr[end];
            int i = start - 1;

            for (int j = start; j <= end - 1; j++)
            {
                if (arr[j] <= p)
                {
                    i++;
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            temp = arr[i + 1];
            arr[i + 1] = arr[end];
            arr[end] = temp;
            return i + 1;
        }
    }
}
