namespace IshTap.Business.Exceptions;

public class LogoutFailException : Exception
{
    public LogoutFailException(string? message) : base(message)
    {
    }
}
