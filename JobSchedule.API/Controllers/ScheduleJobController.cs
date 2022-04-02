using JobSchedule.External;
using JobSchedule.API.Controllers;
using JobSchedule.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;

namespace JobSchedule.API.Controllers
{
    /// <summary>
    ///  class ScheduledJobController
    /// </summary>
    /// <seealso cref="JobSchedule.API.Controllers.ControllerBaseEx" />
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledJobController : ControllerBaseEx
    {
        /// <summary>
        /// The sorting channel
        /// </summary>
        private readonly ISortingChannel _sortingChannel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledJobController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sortingChannel">The sorting channel.</param>
        public ScheduledJobController(ILogger logger, ISortingChannel sortingChannel)
        : base((logger))
        {
            _sortingChannel = sortingChannel;
        }

        /// <summary>
        /// Gets all scheduled job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<JobItem> GetAllScheduledJob()
        {
            LogInfo(LogEventMap.WebApi_ScheduledJob_AllJobStatus, null, "Retreiving all jobs metadata.");
            return _sortingChannel.GetAllJob();
        }

        /// <summary>
        /// Gets the scheduled job by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public JobItem GetScheduledJobById(string id)
        {

            LogInfo(LogEventMap.WebApi_ScheduledJob_SpecificJobStatus, id, $"Querying in collections");

            var jobItem = _sortingChannel.GetJobById(id);

            if (jobItem == null)
            {
                LogInfo(LogEventMap.WebApi_ScheduledJob_SpecificJob_NotFound, id, $"Item not found in collection.");
                return null;
            }

            if (jobItem?.Status == JobStatus.Completed)
            {
                return jobItem;
            }
            else
            {
                return new JobItem(jobItem);
            }
        }

        /// <summary>
        /// Accepts array of integers and schedule them for sorting.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        /// JobItem
        /// </returns>
        [HttpPost]
        //      [Route("numbers")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public string AddScheduledJob(int[] items)
        {
            var jobItem = new JobItem(items);

            LogInfo(LogEventMap.WebApi_ScheduledJob_ItemReceived, jobItem.Id, $"Item received with total count {items?.Length}");

            if (jobItem.Status == JobStatus.Rejected)//If Items is empty.
            {
                LogInfo(LogEventMap.WebApi_ScheduledJob_ItemReceived_IsEmpty, jobItem.Id, $"Item is empty. It's still being persisted.");
                _sortingChannel.AddItemAsync(jobItem);
                return jobItem.Id;
            }
            if (jobItem.Status == JobStatus.Completed)//If Items is has only one index.
            {
                LogInfo(LogEventMap.WebApi_ScheduledJob_ItemReceived_Returned, jobItem.Id, $"Item only has one value.It's still being persisted. Item doesn't need processing and returning as Job Completed.");
                _sortingChannel.AddItemAsync(jobItem);
                return jobItem.Id;
            }

            _sortingChannel.AddItemAsync(jobItem);
            LogInfo(LogEventMap.WebApi_ScheduledJob_ItemReceived_Scheduled, jobItem.Id, $"Item has been scheduled.");

            return jobItem.Id;
        }
    }
}