namespace IshTap.Business.Exceptions;

public class VerificationFailException : Exception
{
    public VerificationFailException(string? message) : base(message)
    {
    }
}
