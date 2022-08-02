namespace TrafficLight.Core;

public class AsyncVehicle<T, TV>:IVehicle
{
    public T Parameter { get; init; }
    public TaskCompletionSource<TV> TaskCompletionSource { get; init; }
    public Func<T, Task<TV>> Callback { get; init; }

    /// <inheritdoc />
    public void Run()
    {
        Callback(Parameter).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                TaskCompletionSource.TrySetException(task.Exception);
            }
            else
            {
                TaskCompletionSource.TrySetResult(task.Result);
            }
        });
    }
}