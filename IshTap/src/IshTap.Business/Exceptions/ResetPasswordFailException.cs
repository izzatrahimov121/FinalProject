namespace IshTap.Business.Exceptions;

public class ResetPasswordFailException : Exception
{
    public ResetPasswordFailException(string? message) : base(message)
    {
    }
}
