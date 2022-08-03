using TrafficLight.Exceptions;

namespace TrafficLight.Core;

public class RoadLine : IRoadLine
{
    
    private int _position = 0;
    private SemaphoreSlim _semaphore = new(1);
    private Dictionary<int, IVehicle> _road = new();

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
            _road.Clear();
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
            if (_road.ContainsKey(_position))
            {
                RunVehicle(_position);
                ClearVehicleFromRoad( _position);
                _position++;
                _semaphore.Release();
                CheckAndRunPosition();
            }
        }
        finally
        {
           
            _semaphore.Release();
        }
    }

    private void ClearVehicleFromRoad(int position)
    {
        _road.Remove(position);
    }

    private void RunVehicle(int position)
    {
        var vehicle = _road[position];
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
            _road.Add(position, vehicle);
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
            _road.Add(position, vehicle);
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