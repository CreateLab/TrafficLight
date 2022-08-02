using TrafficLight.Exceptions;

namespace TrafficLight.Core;

public class RoadLine : IRoadLine
{
    private Dictionary<int, IVehicle> _asyncTasks = new();
    private int _position = 0;
    private SemaphoreSlim _semaphore = new(1);
    private Dictionary<int, IVehicle> _task = new();

    /// <inheritdoc />
    public Task<TV> AddTask<T, TV>(int position, T parameter, Func<T, TV> callback)
    {
        if (position < 0)
            throw new NegativePositionException("Position cannot be negative");
        var tcs = new TaskCompletionSource<TV>();
        AddTaskToQueue(position, parameter, tcs, callback);
        CheckAndRunPosition();
        return tcs.Task;
    }

    /// <inheritdoc />
    public Task<TV> AddTaskAsync<T, TV>(int position, T parameter, Func<T, Task<TV>> callback)
    {
        if (position < 0)
            throw new NegativePositionException("Position cannot be negative");
        var tcs = new TaskCompletionSource<TV>();
        AddAsyncTaskToQueue(position, parameter, tcs, callback);
        CheckAndRunPosition();
        return tcs.Task;
    }

    /// <inheritdoc />
    public void Clear()
    {
        _semaphore.Wait();
        try
        {
            _position = 0;
            _task.Clear();
            _asyncTasks.Clear();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void CheckAndRunPosition()
    {
        _semaphore.Wait();
        try
        {
            if (_task.ContainsKey(_position))
            {
                RunVehicle(_task, _position);
            }
            else
            {
                if (_asyncTasks.ContainsKey(_position))
                {
                    RunVehicle(_asyncTasks, _position);
                }
            }
        }
        finally
        {
            _position++;
            _semaphore.Release();
        }
    }

    private void RunVehicle(IReadOnlyDictionary<int, IVehicle> dictionary, int position)
    {
        var vehicle = dictionary[position];
        vehicle.Run();
    }

    private void AddTaskToQueue<T, TV>(int position, T parameter,
        TaskCompletionSource<TV> tcs, Func<T, TV> callback)
    {
        var vehicle = new Vehicle<T, TV>
        {
            Parameter = parameter,
            TaskCompletionSource = tcs,
            Callback = callback
        };
        _semaphore.Wait();
        try
        {
            _task.Add(position, vehicle);
        }
        catch (ArgumentException)
        {
            throw new PositionLineException($"this position {position} is already occupied");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void AddAsyncTaskToQueue<T, TV>(int position, T parameter, TaskCompletionSource<TV> tcs,
        Func<T, Task<TV>> callback)
    {
        var vehicle = new AsyncVehicle<T, TV>
        {
            Parameter = parameter,
            TaskCompletionSource = tcs,
            Callback = callback
        };
        _semaphore.Wait();
        try
        {
            _asyncTasks.Add(position, vehicle);
        }
        catch (ArgumentException)
        {
            throw new PositionLineException($"this position {position} is already occupied");
        }
        finally
        {
            _semaphore.Release();
        }
    }
}