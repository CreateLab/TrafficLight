namespace TrafficLight.Core;

public class Vehicle<T, TV>:IVehicle
{
    public T Parameter { get; init; }
    public TaskCompletionSource<TV> TaskCompletionSource { get; init; }
    public Func<T, TV> Callback { get; init; }

    /// <inheritdoc />
    public void Run()
    {
        var result = Callback(Parameter);
        TaskCompletionSource.SetResult(result);
    }
}