namespace TrafficLight.Core;

public interface IVehicle
{
    /// <summary>
    /// Vehicle's current state
    /// </summary>
    public bool IsAsynchronous { get; }
    
    /// <summary>
    /// Need to reset postion in roadline
    /// </summary>
    public bool NeedClear { get; }
    /// <summary>
    /// Run task
    /// </summary>
    void Run();
    
}