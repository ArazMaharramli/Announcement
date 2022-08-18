using System;
using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface ITaskScheduler
    {
        string ExecuteAt(Expression<Action> func, DateTime executionTime);
        string ExecuteAt(Expression<Action> func, string cronExpression);

        void CancelTask(string taskId);
    }
}