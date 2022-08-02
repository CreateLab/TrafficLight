namespace TrafficLight.Core;

public interface ITrafficLight
{
    /// <summary>
    /// Create roadline
    /// </summary>
    /// <param name="roadLineName"></param>
    /// <returns></returns>
    IRoadLine GetRoadLine(string roadLineName);
}