namespace TrafficLight.Core;

public interface IVehicle
{
    /// <summary>
    /// Vehicle's current state
    /// </summary>
    public bool IsAsynchronous { get; }
    /// <summary>
    /// Run task
    /// </summary>
    void Run();
    
}