namespace TrafficLight.Core;

public interface IRoadLine
{
    /// <summary>
    ///  Add task to roadline
    /// </summary>
    /// <param name="position"></param>
    /// <param name="parameter"></param>
    /// <param name="callback"></param>
    /// <param name="needClear"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <returns></returns>
    Task<TV> AddTask<T, TV>(int position, T parameter, Func<T, TV> callback, bool needClear = false);

    /// <summary>
    /// Add and run task to roadline
    /// </summary>
    /// <param name="position"></param>
    /// <param name="parameter"></param>
    /// <param name="callback"></param>
    /// <param name="needClear"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <returns></returns>
    Task<TV> AddTaskAsync<T, TV>(int position, T parameter, Func<T, Task<TV>> callback, bool needClear = false);
    
    /// <summary>
    /// Delete all tasks from roadline
    /// </summary>
    void Clear();
}