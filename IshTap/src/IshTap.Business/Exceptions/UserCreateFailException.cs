namespace IshTap.Business.Exceptions;

public class UserCreateFailException : Exception
{
    public UserCreateFailException(string? message) : base(message)
    {
    }
}
