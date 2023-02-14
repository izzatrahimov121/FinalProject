namespace IshTap.Business.Exceptions;

public class AuthFailException : Exception
{
    public AuthFailException(string? message) : base(message)
    {
    }
}
