using System;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Hangfire;

namespace Infrastructure.Common
{
    public class TaskScheduler : ITaskScheduler
    {
        private readonly IBackgroundJobClient _backgroundTaskManager;
        private readonly IRecurringJobManager _recurringJobManager;

        public TaskScheduler(IBackgroundJobClient backgroundTaskManager, IRecurringJobManager recurringJobManager)
        {
            _backgroundTaskManager = backgroundTaskManager;
            _recurringJobManager = recurringJobManager;
        }

        public void CancelTask(string taskId)
        {
            _recurringJobManager.RemoveIfExists(taskId);
            _backgroundTaskManager.Delete(taskId);
        }

        public string ExecuteAt(Expression<Action> func, DateTime executionTime)
        {
            return _backgroundTaskManager.Schedule(func, executionTime);
        }

        public string ExecuteAt(Expression<Action> func, string cronExpression)
        {
            var jobId = Guid.NewGuid().ToString();
            _recurringJobManager.AddOrUpdate(jobId, func, cronExpression);
            return jobId;
        }
    }
}
