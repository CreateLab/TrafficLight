using Moq.AutoMock;
using Xunit;

namespace TrafficLight.Tests;

public class TaskRunTests
{
    private readonly AutoMocker _mocker = new();
    [Fact]
    public async Task CheckClearTask()
    {
        // Arrange
        var tasks = new List<Task>();
        var Queue = new Queue<string>();
        var trafficLight = _mocker.CreateInstance<Core.TrafficLight>();
        var roadLine = trafficLight.GetRoadLine(nameof(CheckClearTask));

        // Act
        var addTask2 = roadLine.AddTask(2, 2, x =>
        {
            Queue.Enqueue(x.ToString());
            return x;
        });
        
        var addTask1 = roadLine.AddTask(1, 1, x =>
        {
            Queue.Enqueue(x.ToString());
            return x;
        });
        
        var addTask0 = roadLine.AddTask(0, 0, x =>
        {
            Queue.Enqueue(x.ToString());
            return x;
        });
        
        tasks.Add(addTask0);
        tasks.Add(addTask1);
        tasks.Add(addTask2);
        
        await Task.WhenAll(tasks);
        
        // Assert
        Assert.Equal(3, Queue.Count);
        Assert.Equal("0", Queue.Dequeue());
        Assert.Equal("1", Queue.Dequeue());
        Assert.Equal("2", Queue.Dequeue());
    }
}