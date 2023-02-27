namespace IshTap.Business.Exceptions;

public class TimeIsOverException : Exception
{
    public TimeIsOverException(string? message) : base(message)
    {
    }
}
