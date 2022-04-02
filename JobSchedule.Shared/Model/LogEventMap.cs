namespace JobSchedule.Shared.Model
{
    /// <summary>
    /// enum LogEventMap
    /// </summary>
    public enum LogEventMap
    {
        /// <summary>
        /// The hosting service started
        /// </summary>
        HostingService_Started = 1,
        /// <summary>
        /// The hosting service item received
        /// </summary>
        HostingService_ItemReceived,
        /// <summary>
        /// The hosting service item sorting in progress
        /// </summary>
        HostingService_ItemSortingInProgress,
        /// <summary>
        /// The hosting service item sorting completed
        /// </summary>
        HostingService_ItemSortingCompleted,

        /// <summary>
        /// The web API scheduled job all job status
        /// </summary>
        WebApi_ScheduledJob_AllJobStatus = 1000,
        /// <summary>
        /// The web API scheduled job specific job status
        /// </summary>
        WebApi_ScheduledJob_SpecificJobStatus,
        /// <summary>
        /// The web API scheduled job specific job not found
        /// </summary>
        WebApi_ScheduledJob_SpecificJob_NotFound,

        /// <summary>
        /// The web API scheduled job item received
        /// </summary>
        WebApi_ScheduledJob_ItemReceived = 2000,
        /// <summary>
        /// The web API scheduled job item received is empty
        /// </summary>
        WebApi_ScheduledJob_ItemReceived_IsEmpty,
        /// <summary>
        /// The web API scheduled job item received returned
        /// </summary>
        WebApi_ScheduledJob_ItemReceived_Returned,
        /// <summary>
        /// The web API scheduled job item received scheduled
        /// </summary>
        WebApi_ScheduledJob_ItemReceived_Scheduled,

        /// <summary>
        /// The sort service list received
        /// </summary>
        SortService_ListReceived = 10000,
        /// <summary>
        /// The sort service list received sent
        /// </summary>
        SortService_ListReceivedSent,
        /// <summary>
        /// The sort service list enqueued
        /// </summary>
        SortService_ListEnqueued,
        /// <summary>
        /// The sort service in progress
        /// </summary>
        SortService_InProgress,
        /// <summary>
        /// The sort service completed
        /// </summary>
        SortService_Completed
    }
}