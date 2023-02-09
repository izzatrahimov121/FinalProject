namespace IshTap.Business.Exceptions;

public class IncorrectFileSizeException : Exception
{
    public IncorrectFileSizeException(string? message) : base(message)
    {
    }
}
