namespace TrafficLight.Core;

public class TrafficLight:ITrafficLight
{
    private Dictionary<string, RoadLine> _roadLines;

  
    /// <inheritdoc />
    public IRoadLine GetRoadLine(string roadLineName)
    {
        if (!_roadLines.ContainsKey(roadLineName))
        {
            _roadLines.Add(roadLineName, new RoadLine());
        }
        
        return _roadLines[roadLineName];
    }
}