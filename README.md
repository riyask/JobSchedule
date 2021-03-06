# Job scheduling

## The problem
To simulate a realistic problem, This small repository will be implementing a minimal job scheduling solution exposed via a web API. In short, This solution would make it possible for clients to enqueue an array of numbers to be sorted in the background and to query the state of any previously enqueued job.

For sake of minimizing the complexity, we are choosing to sort an array but it could be any time consuming process (eg: Image processing & facial regonition/mapping, File Uploading, Data Mining etc.). We have also chosen to use In-memory object instead of database to keep it simple and not dependent on any back-end (eg. SQL). 

### Functional requirements
Develop a web API that supports the following operations:
1. The client can enqueue a new job, by providing an unsorted array of numbers as input
2. The client can retrieve an overview of all jobs (both pending and completed)
3. The client can retrieve a specific job by its ID (see below), including the output (sorted array) if the job has completed

A job takes an unsorted array of numbers as input, sorts the input using an algorithm of your choice, and outputs the sorted array. Apart from the input and output arrays a job It also includes the following metadata: 
1. Id - a unique GUID assigned by the application
2. EnqueuedUtcTimeStamp - when was the job enqueued.
3. CompletedUtcTimeStamp - when was the job completed.
4. Duration - How much time did it take to execute the job.
5. Status - Status of the job (eg: Completed, Pending etc.)

Focus area for this solution is in JobSchedule.External where it's using Channel approach for running async background service.

#### Delivery includes
1. Web API including background job processing.
2. Logging features for background processing and web controller events and middleware logger. 
3. StarterKit to easy bootstrap the common features like swagger and more can be integrated as part of bootstrap solution. 
4. Unit Test and Integration test covering key components.


#### Pre-Requisite:
1. Visual Studio 2019 with common packages installed (eg: .Net Framework / .Net Core 3.1 etc.) & nuget feed.

#### Solution consists of following projects
1. Jobschedule.API : .Net Core 3.1 Web Api based project. It also includes Background service within for the sake of simplicity and demo purposes. At later stage, Background service can be extracted and hosted as independent background service.
2. JobSchedule.Shared : Containing all the common/shared models which can be used across the projects.
3. JobSchedule.External : Containing classes to retreive data which might be fetched via some other external services etc.
  3.1 JobSchedule.External.UnitTest : Unit Test Project
4. Jobschedule.Data : Containing all methods to store and get data in inmemory
4 JobSchedule.IntegrationTest : Integration Test of Web Api Project (JobSchedule) with use case of common features being covered & acceptance criteria.

#### How to build:

1. Clone the project using Visual Studio 2019.
2. Go to menu -> Build -> Build Solution.
3. Go to menu -> Test -> Run All Tests
4. Hit F5 (Run) or go to menu -> Debug -> Start Debugging
  4.1 Please ensure your startup solution is JobSchedule
  4.2 Please ensure your running configuration profile is 'JobSchedule - Development'. Staging and Production environment will also be able to run but please do not run under IIS Projects.
5. Once you run, it should open the web browser with url : http://localhost:30680/swagger/index.html 

#### Design Process
1. projects libraries are small and independent as possible.
2. Keeping reusable code to be developed using class library so later on we can push them into artifactory as nuget packages and consume directly from nuget packages instead of referencing in project dependency manually.
3. Using Industry best practices to keep the seperation of concerns.
4. Web Api service is running Background Service.
5. Unit testing and integration testings are done by XUnit and Moq
