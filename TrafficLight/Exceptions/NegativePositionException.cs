namespace TrafficLight.Exceptions;

public class NegativePositionException : Exception
{
    public NegativePositionException(string message)
        : base(message)
    {
    }

    public NegativePositionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}