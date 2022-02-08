namespace Shelf.Pi.Core;


public static class TaskExtensions
{
    // Will run until cancelled, then hides the TaskCanceledException. I'm sure this is wrong to do this, but baby it feels so right....
    public static Task OrUntilCanceled(this Task task, CancellationToken cancellationToken)
        => task.WaitAsync(cancellationToken).ContinueWith(tsk => tsk.Exception?.Handle(ex => ex is TaskCanceledException));
}