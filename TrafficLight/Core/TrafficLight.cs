namespace TrafficLight.Core;

public class TrafficLight : ITrafficLight
{
    private Dictionary<string, RoadLine> _roadLines = new();
    private static readonly object _lock = new();

    /// <inheritdoc />
    public IRoadLine GetRoadLine(string roadLineName)
    {
        lock (_lock)
        {
            if (!_roadLines.ContainsKey(roadLineName))
            {

                _roadLines.Add(roadLineName, new RoadLine());
            }
        }

        return _roadLines[roadLineName];
    }
}