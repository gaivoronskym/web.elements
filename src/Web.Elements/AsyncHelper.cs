namespace Web.Elements;

internal static class AsyncHelper
{
    private static readonly TaskFactory myTaskFactory = new 
        TaskFactory(CancellationToken.None, 
            TaskCreationOptions.None, 
            TaskContinuationOptions.None, 
            TaskScheduler.Default);

    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return AsyncHelper.myTaskFactory
            .StartNew<Task<TResult>>(func)
            .Unwrap<TResult>()
            .GetAwaiter()
            .GetResult();
    }

    public static void RunSync(Func<Task> func)
    {
        AsyncHelper.myTaskFactory
            .StartNew<Task>(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}