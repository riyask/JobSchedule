using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSchedule.Shared.Model
{
    /// <summary>
    /// class JobItem
    /// </summary>
    public class JobItem
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the enqueued UTC time stamp.
        /// </summary>
        /// <value>
        /// The enqueued UTC time stamp.
        /// </value>
        public DateTime? EnqueuedUtcTimeStamp { get; set; }//Define + Agree on defination of Enqueue.
        /// <summary>
        /// Gets or sets the completed UTC time stamp.
        /// </summary>
        /// <value>
        /// The completed UTC time stamp.
        /// </value>
        public DateTime? CompletedUtcTimeStamp { get; set; }//Define + Agree on defination of Completed.
        /// <summary>
        /// Gets the duration of the execute.
        /// </summary>
        /// <value>
        /// The duration of the execute.
        /// </value>
        public TimeSpan? ExecuteDuration => CompletedUtcTimeStamp - EnqueuedUtcTimeStamp;
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public JobStatus Status { get; set; }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<int> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobItem"/> class.
        /// </summary>
        public JobItem()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobItem"/> class.
        /// </summary>
        /// <param name="jobItem">The job item.</param>
        public JobItem(JobItem jobItem)
        {
            Id = jobItem.Id;
            Status = jobItem.Status;
            EnqueuedUtcTimeStamp = jobItem?.EnqueuedUtcTimeStamp;
            CompletedUtcTimeStamp = jobItem?.EnqueuedUtcTimeStamp;
            Items = jobItem.Items;
        }


        /// <summary>
        /// Updates the job status.
        /// </summary>
        /// <param name="jobStatus">The job status.</param>
        public void UpdateJobStatus(JobStatus jobStatus)
        {
            Status = jobStatus;

            if (jobStatus == JobStatus.Completed
                || jobStatus == JobStatus.Failed
                || jobStatus == JobStatus.Rejected)
            {
                CompletedUtcTimeStamp = DateTime.UtcNow;
            }

        }


        /// <summary>
        /// Initialize with collections of array
        /// </summary>
        /// <param name="items">Array of string</param>
        public JobItem(int[] items)
        {
            Initialize();

            Items = new List<int>(items);

            if (Items?.Count() < 1)
            {
                UpdateJobStatus(JobStatus.Rejected);
            }
            else if (Items?.Count() == 1)
            {
                UpdateJobStatus(JobStatus.Completed);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {

            Id = Guid.NewGuid().ToString();
            Status = JobStatus.Pending;
            EnqueuedUtcTimeStamp = DateTime.UtcNow;
        }

    }
}