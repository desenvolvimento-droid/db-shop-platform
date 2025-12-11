using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using MediatR;
using Shop.Dispatcher.Interfaces;

namespace Shop.Dispatcher.BackgorundQueue;

public class DeadLetterFilter(IBackgroundJobClient jobs) : IApplyStateFilter
{
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is FailedState failed)
        {
            if (context.BackgroundJob.Job.Args[0] is INotification domainEvent)
            {
                jobs.Enqueue<IDeadLetterBackgroundQueue>(x =>
                    x.ProcessAsync(domainEvent, failed.Exception.Message, failed.Exception.StackTrace ?? "")
                );
            }
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction) { }
}
