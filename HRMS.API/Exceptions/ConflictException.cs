namespace HRMS.API.Exceptions;

// Used for duplicate or conflicting data
public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, 409)
    {
    }
}