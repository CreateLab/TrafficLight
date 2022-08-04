using Moq.AutoMock;
using TrafficLight.Exceptions;
using Xunit;

namespace TrafficLight.Tests;

public class TaskRunTests
{
    private readonly AutoMocker _mocker = new();

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(2, 1, 0)]
    [InlineData(0, 2, 1)]
    [InlineData(1, 2, 0)]
    public async Task CheckClearTask(int a, int b, int c)
    {
        // Arrange
        var tasks = new List<Task>();
        var queue = new Queue<string>();
        var trafficLight = _mocker.CreateInstance<Core.TrafficLight>();
        var roadLine = trafficLight.GetRoadLine(nameof(CheckClearTask));

        // Act
        var addTask2 = roadLine.AddTask(a, a, x =>
        {
            queue.Enqueue(x.ToString());
            return x;
        }, false);

        var addTask1 = roadLine.AddTask(b, b, x =>
        {
            queue.Enqueue(x.ToString());
            return x;
        }, false);

        var addTask0 = roadLine.AddTask(c, c, x =>
        {
            queue.Enqueue(x.ToString());
            return x;
        }, false);

        tasks.Add(addTask0);
        tasks.Add(addTask1);
        tasks.Add(addTask2);

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("0", queue.Dequeue());
        Assert.Equal("1", queue.Dequeue());
        Assert.Equal("2", queue.Dequeue());
    }

    [Fact]
    public async Task RunDoublePosition()
    {
        // Arrange
        var tasks = new List<Task>();
        var queue = new Queue<string>();
        var trafficLight = _mocker.CreateInstance<Core.TrafficLight>();
        var roadLine = trafficLight.GetRoadLine(nameof(CheckClearTask));
        Exception e = null;

        // Act
        try
        {
            var addTask2 = roadLine.AddTask(1, 1, x =>
            {
                queue.Enqueue(x.ToString());
                return x;
            }, false);

            var addTask1 = roadLine.AddTask(1, 1, x =>
            {
                queue.Enqueue(x.ToString());
                return x;
            }, false);


            tasks.Add(addTask1);
            tasks.Add(addTask2);

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            e = ex;
        }

        // Assert
        Assert.NotNull(e);
        Assert.True(e is PositionLineException);
    }


    [Fact]
    public async Task RunNegativePosition()
    {
        // Arrange
        var tasks = new List<Task>();
        var queue = new Queue<string>();
        var trafficLight = _mocker.CreateInstance<Core.TrafficLight>();
        var roadLine = trafficLight.GetRoadLine(nameof(CheckClearTask));
        Exception e = null;

        // Act
        try
        {
            var addTask2 = roadLine.AddTask(-1, -1, x =>
            {
                queue.Enqueue(x.ToString());
                return x;
            }, false);


            tasks.Add(addTask2);

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            e = ex;
        }

        // Assert
        Assert.NotNull(e);
        Assert.True(e is NegativePositionException);
    }


    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(2, 1, 0)]
    [InlineData(0, 2, 1)]
    [InlineData(1, 2, 0)]
    public async Task CheckClearTaskWithDelay(int a, int b, int c)
    {
        // Arrange
        var tasks = new List<Task>();
        var queue = new Queue<string>();
        var trafficLight = _mocker.CreateInstance<Core.TrafficLight>();
        var roadLine = trafficLight.GetRoadLine(nameof(CheckClearTask));

        // Act
        var addTask2 = roadLine.AddTask(a, a, x =>
        {
            Thread.Sleep(1000);
            queue.Enqueue(x.ToString());
            return x;
        });

        var addTask1 = roadLine.AddTask(b, b, x =>
        {
            Thread.Sleep(1000);
            queue.Enqueue(x.ToString());
            return x;
        });

        var addTask0 = roadLine.AddTask(c, c, x =>
        {
            Thread.Sleep(1000);
            queue.Enqueue(x.ToString());
            return x;
        });

        tasks.Add(addTask0);
        tasks.Add(addTask1);
        tasks.Add(addTask2);

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("0", queue.Dequeue());
        Assert.Equal("1", queue.Dequeue());
        Assert.Equal("2", queue.Dequeue());
    }


}