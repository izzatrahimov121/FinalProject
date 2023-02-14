namespace IshTap.Business.Exceptions;

public class RoleCreateFailException : Exception
{
    public RoleCreateFailException(string? message) : base(message)
    {
    }
}
